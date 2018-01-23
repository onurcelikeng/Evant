using Evant.Contracts.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Helpers
{
    public class BaseController : Controller
    {
        public override OkObjectResult Ok(object value)
        {
            var response = new ResultDTO<object>()
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "",
                Data = value
            };
            return base.Ok(response);
        }

        public override NotFoundObjectResult NotFound(object value)
        {
            var response = new ResultDTO<object>()
            {
                StatusCode = 404,
                IsSuccess = false,
                Message = value.ToString(),
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
