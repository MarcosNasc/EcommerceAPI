using AutoMapper;
using Ecommerce.API.Controllers;
using Ecommerce.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.V2.Controllers;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TesteController : MainController
{
    public TesteController(IMapper mapper, INotificator notificator, IUser appUser) : base(mapper, notificator, appUser)
    {
    }

    [HttpGet]
    public string Teste()
    {
        return "V2";
    }
}