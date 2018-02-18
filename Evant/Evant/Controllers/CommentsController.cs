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

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/comments")]
    public class CommentsController : BaseController
    {
        private readonly ICommentRepository _commentRepo;
        private readonly ILogHelper _logHelper;


        public CommentsController(ICommentRepository commentRepo,
            ILogHelper logHelper)
        {
            _commentRepo = commentRepo;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet("{eventId}")]
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
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    return Ok(commnets);
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Comments", 500, "GetComments", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Eksik bilgi girdiniz.");
                }

                var entity = new Comment()
                {
                    Id = new Guid(),
                    EventId = model.EventId,
                    UserId = User.GetUserId()
                };

                var response = await _commentRepo.Add(entity);
                if (response)
                {
                    return Ok("Yorum eklendi.");
                }
                else
                {
                    return BadRequest("Yorum eklenemedi.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Comments", 500, "AddComment", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var comment = await _commentRepo.First(c => c.Id == commentId && c.UserId == userId);
                if (comment == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }

                var response = await _commentRepo.Delete(comment);
                if (response)
                {
                    return Ok("yorum silindi.");
                }
                else
                {
                    return BadRequest("Yorum silinemedi.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Comments", 500, "DeleteComment", ex.Message);
                return null;
            }
        }

    }
}
