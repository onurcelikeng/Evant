using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace Evant.Helpers
{
    public sealed class LogHelper : ILogHelper
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRepository<Log> _logRepo;


        public LogHelper(IHttpContextAccessor accessor,
            IRepository<Log> logRepo)
        {
            _accessor = accessor;
            _logRepo = logRepo;
        }


        public async void Log(string controller, int statusCode, string action, string ex = null, string message = null)
        {
            if(statusCode == 500)
            {
                SlackHelper slackHelper = new SlackHelper();
                string content = "Controller: " + controller + ", Status: " + statusCode + " Action: " + action + " ex: " + ex;
                slackHelper.PostMessage(content);
            }

            var entity = new Log()
            {
                Id = new Guid(),
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Controller = controller,
                StatusCode = statusCode,
                Action = action,
                Exception = ex,
                Message = message
            };

            var response = await _logRepo.Add(entity);
        }

    }
}
