using AutoMapper;
using Ecommerce.BLL.Interfaces;
using Ecommerce.BLL.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly INotificator _notificator;
        protected readonly IUser _appUser;
        
        protected Guid UserId { get; private set; }
        protected bool UserAuthenticated { get; private set; }
        
        protected MainController(IMapper mapper
                                 ,INotificator notificator  
                                 ,IUser appUser
        )
        {
            _mapper = mapper;
            _notificator = notificator;
            _appUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UserId = appUser.GetUserId();
                UserAuthenticated = true;
            }
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyErrorInvalidModel(modelState);
            return CustomResponse();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new
                {
                    sucess = true,
                    data = result,

                });
            }

            return BadRequest(new
            {
                sucess = false,
                erros = _notificator.GetNotifications().Select(n => n.Message)
            });
        }

        protected bool ValidOperation()
        {
            return !_notificator.HasNotification();
        }

        protected void NotifyErrorInvalidModel(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            foreach (var erro in erros)
            {
                var errorMessage =  erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(errorMessage);
            }
        }

        protected void NotifyError(string message)
        {
            _notificator.Handle(new Notification(message));
        }
    }
}