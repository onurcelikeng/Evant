using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects.Comment;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/comments")]
    public class CommentsController : BaseController
    {
        private readonly IRepository<Comment> _commentRepo;


        public CommentsController(IRepository<Comment> commentRepo)
        {
            _commentRepo = commentRepo;
        }


        [Authorize]
        [HttpGet("{eventId}")]
        public IActionResult GetComments([FromRoute] Guid eventId)
        {
            var commnets = _commentRepo.Where(c => c.EventId == eventId).Select(c => new CommentDetailDTO()
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
            });

            return Ok(commnets);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddComment([FromBody] CommentDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Eksik bilgi girdiniz.");

            Guid userId = User.GetUserId();

            var comment = new Comment()
            {
                Id = new Guid(),
                EventId = model.EventId,
                UserId = userId
            };

            var response = _commentRepo.Insert(comment);
            if (response)
            {
                return Ok("Yorum eklendi.");
            }
            else
            {
                return BadRequest("Yorum eklenemedi.");
            }
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment([FromRoute] Guid commentId)
        {
            var comment = _commentRepo.First(c => c.Id == commentId);
            if (comment == null)
                return NotFound("Yorum bulunamadı.");

            var response = _commentRepo.Delete(comment);
            if (response)
            {
                return Ok("yorum silindi.");
            }
            else
            {
                return BadRequest("Yorum silinemedi.");
            }
        }

    }
}
