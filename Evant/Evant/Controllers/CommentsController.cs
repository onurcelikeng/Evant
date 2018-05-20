using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Comment;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Evant.Constants.GameConstant;

namespace Evant.Controllers
{
    [Authorize]
    [Route("api/comments")]
    public class CommentsController : BaseController
    {
        private readonly INotificationHelper _notificationHelper;
        private readonly ICommentRepository _commentRepo;
        private readonly IEventRepository _eventRepo;
        private readonly IGameHelper _gameHelper;
        private readonly ILogHelper _logHelper;


        public CommentsController(ICommentRepository commentRepo,
            IEventRepository eventRepo,
            INotificationHelper notificationHelper,
            IGameHelper gameHelper,
            ILogHelper logHelper)
        {
            _commentRepo = commentRepo;
            _eventRepo = eventRepo;
            _notificationHelper = notificationHelper;
            _gameHelper = gameHelper;
            _logHelper = logHelper;
        }


        [HttpGet()]
        [Route("{eventId}")]
        public async Task<IActionResult> GetComments([FromRoute] Guid eventId)
        {
            try
            {
                var commnets = (await _commentRepo.Comments(eventId)).Select(c => new CommentDetailDTO()
                {
                    CommentId = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    User = new UserInfoDTO()
                    {
                        UserId = c.User.Id,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName,
                        PhotoUrl = c.User.Photo
                    }
                }).ToList();

                if (commnets.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(commnets);
            }
            catch (Exception ex)
            {
                _logHelper.Log("CommentsController", 500, "GetComments", ex.Message);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                Guid userId = User.GetUserId();

                var entity = new Comment()
                {
                    Id = new Guid(),
                    UserId = userId,
                    EventId = model.EventId,
                    Content = model.Content
                };

                var response = await _commentRepo.Add(entity);
                if (response)
                {
                    await _gameHelper.Point(userId, GameType.CommentEvent);

                    var @event = await _eventRepo.First(e => e.Id == model.EventId);
                    if (@event != null)
                    {
                        Guid receiverId = @event.UserId;
                        if (!receiverId.Equals(userId))
                            await _notificationHelper.SendCommentNotification(userId, receiverId, @event.Id, model.Content);
                    }

                    return Ok("Yorumunuz eklendi.");
                }
                else
                {
                    return BadRequest("Yorumunuz eklenemedi.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("CommentsController", 500, "AddComment", ex.Message);
                return null;
            }
        }

        [HttpDelete()]
        [Route("{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            try
            {
                var comment = await _commentRepo.First(c => c.Id == commentId && c.UserId == User.GetUserId());
                if (comment == null)
                    return NotFound("Kayıt bulunamadı.");

                var response = await _commentRepo.Delete(comment);
                if (response)
                    return Ok("Yorumunuz silindi.");
                else
                    return BadRequest("Yorumunuz silinemedi.");
            }
            catch (Exception ex)
            {
                _logHelper.Log("CommentsController", 500, "DeleteComment", ex.Message);
                return null;
            }
        }

    }
}
