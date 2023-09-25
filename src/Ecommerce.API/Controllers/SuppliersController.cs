using AutoMapper;
using Ecommerce.API.DTOs;
using Ecommerce.BLL.Entities;
using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : BaseController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;


        public SuppliersController(ISupplierRepository supplierRepository 
                                   ,IMapper mapper
                                   ,ISupplierService supplierService)
            :base(mapper)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetAll()
        {
            var suppliers =  _mapper.Map<IEnumerable<SupplierDTO>>(await _supplierRepository.GetAll());
            return Ok(suppliers);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SupplierDTO>> GetById(Guid id)
        {
            var supplier = _mapper.Map<SupplierDTO>(await _supplierRepository.GetSupplierProductsAddress(id));

            if(supplier is null) return NotFound();

            return  Ok(supplier);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SupplierDTO>> Create(SupplierDTO supplierDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierDTO);

            try
            {
               await _supplierService.Add(supplier);
               return CreatedAtAction(nameof(Create), supplierDTO);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SupplierDTO>> Update(Guid id , SupplierDTO supplierDTO)
        {
            if(id != supplierDTO.Id) return BadRequest(ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierDTO);

            try
            {
                await _supplierService.Update(supplier);
                return Ok(supplierDTO);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierDTO>> Delete(Guid id)
        {
             
            var supplier = await _supplierRepository.GetSupplierAdress(id);

            if (supplier is null) return NotFound();

            try
            {
                await _supplierService.Remove(supplier.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
