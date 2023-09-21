using Ecommerce.API.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ecommerce.API.DTOs
{
    public class ProductDTO
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(200, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(1000, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 2)]
        public string Description { get; set; }
        public IFormFile? ImageUpload { get; set; }
        public string Image { get; set; }
        //[Money]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]
        public decimal Value { get; set; }
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Status")]
        public bool IsActive { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "FieldRequired")]

        [DisplayName("Supplier")]
        public Guid SupplierId { get; set; }
        public SupplierDTO? Supplier { get; set; }
        public IEnumerable<SupplierDTO> Suppliers { get; set; } = new List<SupplierDTO>();
    }
}
