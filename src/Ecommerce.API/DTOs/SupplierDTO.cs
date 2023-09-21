using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Ecommerce.API.Resources;

namespace Ecommerce.API.DTOs
{
    public class SupplierDTO
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 2)]
        public string? Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(15, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 2)]
        public string? Document { get; set; }
        [DisplayName("Tipo")]
        public int SupplierType { get; set; }
        public AddressDTO? Address { get; set; }
        [DisplayName("Status")]
        public bool IsActive { get; set; }
        public IEnumerable<ProductDTO>? Products { get; set; }
    }
}
