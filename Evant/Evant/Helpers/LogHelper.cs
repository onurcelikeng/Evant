using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Interfaces;
using System;
using System.Threading.Tasks;

namespace Evant.Helpers
{
    public sealed class LogHelper : ILogHelper
    {
        private readonly IRepository<Log> _logRepo;


        public LogHelper(IRepository<Log> logRepo)
        {
            _logRepo = logRepo;
        }


        public async void Log(string table, string status, string message = null, string ex = null)
        {
            try
            {
                var newLog = new Log()
                {
                    Id = new Guid(),
                    Ip = "ip-address",
                    Table = table,
                    Status = status,
                    Message = message,
                    Exception = ex,
                };

                var response = await _logRepo.Add(newLog);
            }
            catch (Exception)
            {

            }
        }

    }
}
