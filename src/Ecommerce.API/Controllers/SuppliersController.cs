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
    public class SuppliersController : BaseController
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;


        public SuppliersController(ISupplierService supplierService
                                  ,ISupplierRepository supplierRepository 
                                  ,IAddressRepository addressRepository
                                  ,IMapper mapper
                                  ,INotificator notificator)
            :base(mapper,notificator)
        {
            _supplierService = supplierService;
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
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
        [ClaimsAuthorize("Crud","Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SupplierDTO>> Create(SupplierDTO supplierDTO)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierDTO);

            await _supplierService.Add(supplier);

            return CustomResponse(supplierDTO);
        }

        [HttpPut("{id:guid}")]
        [ClaimsAuthorize("Crud","Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SupplierDTO>> Update(Guid id , SupplierDTO supplierDTO)
        {
            if(id != supplierDTO.Id) return BadRequest(ModelState);

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierDTO);

            await _supplierService.Update(supplier);

            return CustomResponse(supplierDTO);
           
        }

        [HttpDelete("{id:guid}")]
        [ClaimsAuthorize("Crud","Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierDTO>> Delete(Guid id)
        {
             
            var supplier = await _supplierRepository.GetSupplierAdress(id);

            if (supplier is null) return NotFound();

            await _supplierService.Remove(supplier.Id);

            return CustomResponse();

        }

        [HttpGet("getAddressById/{id:guid}")]
        public async Task<AddressDTO> GetAddressById(Guid id)
        {
            var addressDTO = _mapper.Map<AddressDTO>(_addressRepository.GetById(id));
            return addressDTO;
        }

        [HttpPut("UpdateAddress")]
        [ClaimsAuthorize("Crud","Update")]
        public async Task<IActionResult> UpdateAddress(Guid id, AddressDTO addressDTO)
        {
            if (id != addressDTO.Id) return BadRequest();

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var address = _mapper.Map<Address>(addressDTO);
            await _supplierService.UpdateAddress(address);

            return CustomResponse(addressDTO);
        }
    }
}
