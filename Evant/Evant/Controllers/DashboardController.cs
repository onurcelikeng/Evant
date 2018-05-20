using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.Cognitive;
using Evant.Contracts.DataTransferObjects.Business;
using Evant.Contracts.DataTransferObjects.Dashboard;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Evant.NotificationCenter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace Evant.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<UserDevice> _userDeviceRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IEventOperationRepository _eventOperationRepo;
        private readonly IOneSignal _oneSignal;
        private readonly ILogHelper _logHelper;


        public DashboardController(IRepository<User> userRepo,
            IRepository<UserDevice> userDeviceRepo,
            ICommentRepository commentRepo,
            IEventOperationRepository eventOperationRepo,
            IOneSignal oneSignal,
            ILogHelper logHelper)
        {
            _userRepo = userRepo;
            _commentRepo = commentRepo;
            _userDeviceRepo = userDeviceRepo;
            _eventOperationRepo = eventOperationRepo;
            _oneSignal = oneSignal;
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

                    model.Teenager.Ratio = (model.Teenager.Value * users.Count) / 100;
                    model.Young.Ratio = (model.Young.Value * users.Count) / 100;
                    model.Middle.Ratio = (model.Middle.Value * users.Count) / 100;
                    model.Old.Ratio = (model.Old.Value * users.Count) / 100;
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logHelper.Log("DashboardController", 500, "GetUsersAnalyses", ex.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("announcement")]
        public async Task<IActionResult> SendAnnouncement([FromBody] AnnouncementDTO model)
        {
            try
            {
                var userIds = (await _eventOperationRepo.Participants(model.EventId)).Select(u => u.UserId).ToList();
                if (userIds.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                foreach (var id in userIds)
                {
                    var playerIds = (await _userDeviceRepo.Where(d => d.UserId == id && d.IsLoggedin))
                        .Select(t => t.DeviceId)
                        .ToList();
                    
                    _oneSignal.SendNotification(playerIds, model.Message);
                }

                return Ok("Duyuru gönderilmiştir.");
            }
            catch (Exception ex)
            {
                _logHelper.Log("DashboardController", 500, "SendAnnouncement", ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("{eventId}/comments")]
        public async Task<IActionResult> GetCommentsAnalyse([FromRoute] Guid eventId)
        {
            var model = new List<CommentAnalyticsDTO>();

            var comments = await _commentRepo.Comments(eventId);
            if (!comments.IsNullOrEmpty())
            {
                var inputs = new List<Input>();
                foreach (var comment in comments)
                {
                    inputs.Add(new Input()
                    {
                        Id = comment.Id.ToString(),
                        Text = comment.Content
                    });
                    model.Add(new CommentAnalyticsDTO()
                    {
                        CommentId = comment.Id,
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt
                    });
                }

                TextAnalytics _textAnalytics = new TextAnalytics();

                var multiLanguageInputs = new List<MultiLanguageInput>();
                var languageResult = _textAnalytics.DetectLanguage(inputs);
                if (languageResult != null)
                {
                    for (int i = 0; i < languageResult.Documents.Count; i++)
                    {
                        foreach (var item in model)
                        {
                            if (item.CommentId.ToString() == languageResult.Documents[i].Id)
                            {
                                item.Language = languageResult.Documents[i].DetectedLanguages[0].Name;
                                item.LanguageCode = languageResult.Documents[i].DetectedLanguages[0].Iso6391Name;
                            }
                        }

                        multiLanguageInputs.Add(new MultiLanguageInput()
                        {
                            Id = model[i].CommentId.ToString(),
                            Language = model[i].LanguageCode,
                            Text = model[i].Content
                        });
                    }

                    var keyPhrases = _textAnalytics.GetKeyPhrases(multiLanguageInputs);
                    if (keyPhrases != null)
                    {
                        for (int i = 0; i < keyPhrases.Documents.Count; i++)
                        {
                            foreach (var item in model)
                            {
                                if (item.CommentId.ToString() == keyPhrases.Documents[i].Id)
                                {
                                    item.KeyPhrases = keyPhrases.Documents[i].KeyPhrases.ToList();
                                }
                            }
                        }
                    }

                    var sentimentResult = _textAnalytics.GetSentiment(multiLanguageInputs);
                    if (sentimentResult != null)
                    {
                        for (int i = 0; i < sentimentResult.Documents.Count; i++)
                        {
                            foreach (var item in model)
                            {
                                if (item.CommentId.ToString() == sentimentResult.Documents[i].Id)
                                {
                                    item.Sentiment = sentimentResult.Documents[i].Score.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return Ok(model);
        }

        [HttpGet]
        [Route("{eventId}/date")]
        public async Task<IActionResult> GetDayAnalyse([FromRoute] Guid eventId)
        {
            try
            {
                var users = await _eventOperationRepo.Participants(eventId);
                if (users.IsNullOrEmpty())
                    return BadRequest("Kayıt bulunamadı.");

                var dayGroups = users.GroupBy(u => u.CreatedAt.ToShortDateString()).Select(g => g.ToList()).ToList();
                
                var list = new List<DateAnalyticsDTO>();
                foreach (var day in dayGroups)
                {
                    list.Add(new DateAnalyticsDTO()
                    {
                        Date = day[0].CreatedAt.ToShortDateString(),
                        UserCount = day.Count
                    });
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                _logHelper.Log("DashboardController", 500, "GetDayAnalyse", ex.Message);
                return null;
            }
        }

    }
}
