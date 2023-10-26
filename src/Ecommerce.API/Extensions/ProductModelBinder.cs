using Ecommerce.API.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Ecommerce.API.Extensions
{
    public class ProductModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = true
            };

            var ProductImageDTO = JsonSerializer.Deserialize<ProductImageDTO>(bindingContext.ValueProvider.GetValue("product").FirstOrDefault(), serializeOptions);
            ProductImageDTO.ImageUpload = bindingContext.ActionContext.HttpContext.Request.Form.Files.FirstOrDefault();

            bindingContext.Result = ModelBindingResult.Success(ProductImageDTO);
            return Task.CompletedTask;
        }
    }
}