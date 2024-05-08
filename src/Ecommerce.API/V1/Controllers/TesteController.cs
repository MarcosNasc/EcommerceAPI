using AutoMapper;
using Ecommerce.API.Controllers;
using Ecommerce.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.V1.Controllers;

[ApiVersion("1.0",Deprecated = true)]
[Route("api/v{version:apiVersion}/[controller]")]
public class TesteController : MainController
{
    public TesteController(IMapper mapper, INotificator notificator, IUser appUser) : base(mapper, notificator, appUser)
    {
    }

    [HttpGet]
    public string Teste()
    {
        return "V1";
    }
}