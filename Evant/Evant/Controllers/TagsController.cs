using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TagsController : BaseController
    {
        private readonly IRepository<Tag> _tagRepo;


        public TagsController(IRepository<Tag> tagRepo)
        {
            _tagRepo = tagRepo;
        }


        [HttpGet]
        public IActionResult GetTags()
        {
            var tags = _tagRepo.GetAll().Select(t => new TagDTO()
            {
                Id = t.Id,
                Name = t.Name
            });

            return Ok(tags);
        }

        [HttpGet("{id}")]
        public IActionResult GetTag([FromRoute] Guid id)
        {
            var tag = _tagRepo.First(t => t.Id == id);
            if (tag == null)
            {
                return NotFound("Böyle bir tag yok.");
            }

            return Ok(tag);
        }

        [HttpPut]
        public IActionResult PutTag([FromBody] TagDTO tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(null);
            }

            var selectedTag = _tagRepo.First(t => t.Id == tag.Id);
            if (selectedTag == null)
            {
                return NotFound("Tag bulunamadı.");
            }
            else
            {
                selectedTag.Name = tag.Name;
                selectedTag.UpdateAt = DateTime.Now;

                var response = _tagRepo.Update(selectedTag);
                if (response)
                {
                    return Ok("Tag güncellendi.");
                }
                else
                {
                    return BadRequest("Tag güncellenemedi.");
                }
            }
        }

        [HttpPost]
        public IActionResult PostTag([FromBody] TagDTO tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(null);
            }

            var response = _tagRepo.Insert(new Tag()
            {
                Id = new Guid(),
                Name = tag.Name,
                CreatedAt = DateTime.Now
            });
            if (response)
            {
                return Ok("Tag eklendi.");
            }
            else
            {
                return BadRequest("Tag eklenemedi.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTag([FromRoute] Guid id)
        {
            var tag = _tagRepo.First(t => t.Id == id);
            if (tag == null)
            {
                return NotFound("Böyle bir kategori yok.");
            }

            var response = _tagRepo.Delete(tag);
            if (response)
            {
                return Ok("Tag silindi.");
            }
            else
            {
                return BadRequest("Tag silinemedi.");
            }
        }

    }
}