using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Dashboard;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Comment> _commentRepo;
        private readonly IEventOperationRepository _eventOperationRepo;
        private readonly ILogHelper _logHelper;


        public DashboardController(IRepository<User> userRepo,
            IRepository<Comment> commentRepo,
            IEventOperationRepository eventOperationRepo,
            ILogHelper logHelper)
        {
            _userRepo = userRepo;
            _commentRepo = commentRepo;
            _eventOperationRepo = eventOperationRepo;
            _logHelper = logHelper;
        }


        [HttpGet]
        [Route("{eventId}/users")]
        public async Task<IActionResult> GetUsersAnalyses([FromRoute] Guid eventId)
        {
            try
            {
                var model = new UserAnalyticsDTO()
                {
                    Teenager = new T()
                    {
                        Name = "Teenager",
                        Min = 0,
                        Max = 17
                    },
                    Young = new T()
                    {
                        Name = "Young",
                        Min = 18,
                        Max = 65
                    },
                    Middle = new T()
                    {
                        Name = "Middle Aged",
                        Min = 66,
                        Max = 79
                    },
                    Old = new T()
                    {
                        Name = "Old",
                        Min = 80,
                        Max = 99
                    }
                };

                var users = await _eventOperationRepo.Participants(eventId);
                if (!users.IsNullOrEmpty())
                {
                    var today = DateTime.Today;
                    foreach (var user in users)
                    {
                        int age = today.Year - user.User.Birthdate.Year;
                        if (model.Teenager.Min < age && age < model.Teenager.Max) model.Teenager.Value += 1;
                        else if (model.Young.Min < age && age < model.Young.Max) model.Young.Value += 1;
                        else if (model.Middle.Min < age && age < model.Middle.Max) model.Middle.Value += 1;
                        else if (model.Old.Min < age && age < model.Old.Max) model.Old.Value += 1;
                    }

                    model.Teenager.Ratio = (model.Teenager.Value * users.Count) * 100;
                    model.Young.Ratio = (model.Young.Value * users.Count) * 100;
                    model.Middle.Ratio = (model.Middle.Value * users.Count) * 100;
                    model.Old.Ratio = (model.Old.Value * users.Count) * 100;
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logHelper.Log("DashboardController", 500, "GetUsersAnalyses", ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("{eventId}/comments")]
        public async Task<IActionResult> GetComments([FromRoute] Guid eventId)
        {
            //var comments = await _commentRepo.Where(c => c.EventId == eventId)
            return null;
        }

    }
}
