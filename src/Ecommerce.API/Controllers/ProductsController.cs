using AutoMapper;
using Ecommerce.API.DTOs;
using Ecommerce.API.Extensions.Attributes;
using Ecommerce.BLL.Entities;
using Ecommerce.BLL.Interfaces;
using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductService productService
                                 ,IProductRepository productRepository
                                 ,IMapper mapper
                                 ,INotificator notificator) : base(mapper, notificator)
        {
            _productService = productService;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = _mapper.Map<IEnumerable<ProductDTO>>(await _productRepository.GetProductsSuppliers());
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDTO>> GetById(Guid id)
        {
            var product = _mapper.Map<ProductDTO>(await _productRepository.GetById(id));

            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ClaimsAuthorize("Crud","Create")]
        public async Task<ActionResult<ProductDTO>> Create(ProductDTO productDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var product = _mapper.Map<Product>(productDTO);

            var fileName = Guid.NewGuid() + "_" + productDTO.Image;

            if(!FileUpload(productDTO.ImageUpload , fileName))
            {
                return CustomResponse();
            }

            productDTO.Image = fileName;
            await _productService.Add(product);
            return CustomResponse(productDTO);
        }

        [HttpPost("CreateStream")]
        [ClaimsAuthorize("Crud","Create")]
        public async Task<ActionResult<ProductImageDTO>> CreateStream(ProductImageDTO productDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var product = _mapper.Map<Product>(productDTO);

            var imgPrefix = Guid.NewGuid() + "_";

            if (!await FileUpload(productDTO.ImageUpload, imgPrefix))
            {
                return CustomResponse();
            }

            productDTO.Image = imgPrefix + productDTO.ImageUpload.FileName;
            await _productService.Add(product);
            return CustomResponse(productDTO);
        }

        [HttpPut("{id:guid}")]
        [ClaimsAuthorize("Crud","Update")]
        public async Task<ActionResult<ProductDTO>> Update(Guid id,ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                NotifyError("O produto não foi encontrado");
                return CustomResponse(); 
            }

            var productCurrent = _mapper.Map<ProductDTO>(await _productRepository.GetProductSupplier(id));
            productDTO.Image = productCurrent.Image;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if(productDTO.ImageUpload != null)
            {
                var imageName = Guid.NewGuid() + "_" + productDTO.Image;
                if(!FileUpload(productDTO.ImageUpload, imageName))
                {
                    return CustomResponse(ModelState);
                }

                productCurrent.Image = imageName;
            }

            productCurrent.Name = productDTO.Name;
            productCurrent.Description = productDTO.Description;
            productCurrent.Value = productDTO.Value;
            productCurrent.IsActive = productDTO.IsActive;

            var product = _mapper.Map<Product>(productCurrent);

            await _productService.Update(product);
            return CustomResponse(productDTO);
        }

        [HttpDelete("{id:guid}")]
        [ClaimsAuthorize("Crud","Delete")]
        public async Task<ActionResult<ProductDTO>> Delete(Guid id)
        {
            var product = await _productRepository.GetById(id);
            if (product is null) return NotFound();
            await _productService.Remove(product.Id);
            return CustomResponse();
        }

        private bool FileUpload(string file, string fileName)
        {
            var fileDataByteArray = Convert.FromBase64String(file);

            if(string.IsNullOrEmpty(file))
            {
                NotifyError("Forneça uma imagem para este produto!");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

            if(System.IO.File.Exists(filePath))
            {
                NotifyError("Já existe um arquivo com esse nome");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, fileDataByteArray);

            return true;
        }

        private async Task<bool> FileUpload(IFormFile file, string imgPrefix)
        {
            if(file == null || file.Length == 0)
            {
                NotifyError("Forneça uma imagem para este produto!");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", imgPrefix + file.FileName);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("Já existe um arquivo com esse nome");
                return false;
            }

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}
