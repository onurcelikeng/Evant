using Evant.Contracts.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Helpers
{
    public class BaseController : Controller
    {
        public override OkObjectResult Ok(object data)
        {
            //var response = new ResultDTO<object>();
            //if (data.GetType() == typeof(string))
            //{
            //    response = new ResultDTO<object>()
            //    {
            //        StatusCode = 200,
            //        IsSuccess = true,
            //        Message = data.ToString(),
            //        Data = null
            //    };
            //}
            //else
            
                var response = new ResultDTO<object>()
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = "",
                    Data = data
                };
            

            return base.Ok(response);
        }

        public override NotFoundObjectResult NotFound(object message)
        {
            var response = new ResultDTO<object>()
            {
                StatusCode = 404,
                IsSuccess = false,
                Message = message.ToString(),
                Data = null
            };
            return base.NotFound(response);
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            var response = new ResultDTO<object>()
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = error.ToString(),
                Data = null
            };
            return base.BadRequest(response);
        }

    }
}
