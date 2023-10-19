using AutoMapper;
using Ecommerce.API.DTOs;
using Ecommerce.BLL.Entities;
using Ecommerce.BLL.Interfaces;
using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
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

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductDTO>> Update(Guid id,ProductDTO productDTO)
        {
            if(id != productDTO.Id) return BadRequest(ModelState);

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var product = _mapper.Map<Product>(productDTO);

            await _productService.Update(product);
            return CustomResponse(productDTO);
        }

        [HttpDelete("{id:guid}")]
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
    }
}
