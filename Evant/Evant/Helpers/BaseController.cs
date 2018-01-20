using Evant.Contracts.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Helpers
{
    public class BaseController : Controller
    {
        private BaseController() { }

        public static BaseController Instance => new BaseController();


        public IActionResult Result(object data, int statusCode = 200, string message = null)
        {
            var response = new ResultDTO<object>()
            {
                StatusCode = statusCode,
                IsSuccess = statusCode == 200,
                Message = message,
                Data = data
            };

            switch (statusCode)
            {
                case 200:
                    return Ok(response);
                case 400:
                    return BadRequest(response);
                case 404:
                    return NotFound(response);
                default:
                    return BadRequest();
            }
        }

    }
}
