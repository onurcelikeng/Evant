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
    public class TagsController : Controller
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

            return BaseController.Instance.Result(tags, 200);
        }

        [HttpGet("{id}")]
        public IActionResult GetTag([FromRoute] Guid id)
        {
            var tag = _tagRepo.First(t => t.Id == id);
            if (tag == null)
            {
                return BaseController.Instance.Result(null, 404, "Böyle bir tag yok.");
            }

            return BaseController.Instance.Result(tag, 200);
        }

        [HttpPut]
        public IActionResult PutTag([FromBody] TagDTO tag)
        {
            if (!ModelState.IsValid)
            {
                return BaseController.Instance.Result(null, 400);
            }

            var selectedTag = _tagRepo.First(t => t.Id == tag.Id);
            if (selectedTag == null)
            {
                return BaseController.Instance.Result(null, 404, "Tag bulunamadı.");
            }
            else
            {
                selectedTag.Name = tag.Name;
                selectedTag.UpdateAt = DateTime.Now;

                var response = _tagRepo.Update(selectedTag);
                if (response)
                {
                    return BaseController.Instance.Result(null, 200, "Tag güncellendi.");
                }
                else
                {
                    return BaseController.Instance.Result(null, 500, "Tag güncellenemedi.");
                }
            }
        }

        [HttpPost]
        public IActionResult PostTag([FromBody] TagDTO tag)
        {
            if (!ModelState.IsValid)
            {
                return BaseController.Instance.Result(null, 400);
            }

            var response = _tagRepo.Insert(new Tag()
            {
                Id = new Guid(),
                Name = tag.Name,
                CreatedAt = DateTime.Now
            });
            if (response)
            {
                return BaseController.Instance.Result(null, 200, "Tag eklendi.");
            }
            else
            {
                return BaseController.Instance.Result(null, 500, "Tag eklenemedi.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTag([FromRoute] Guid id)
        {
            var tag = _tagRepo.First(t => t.Id == id);
            if (tag == null)
            {
                return BaseController.Instance.Result(null, 404, "Böyle bir kategori yok.");
            }

            var response = _tagRepo.Delete(tag);
            if (response)
            {
                return BaseController.Instance.Result(null, 200, "Tag silindi.");
            }
            else
            {
                return BaseController.Instance.Result(null, 500, "Tag silinemedi.");
            }
        }

    }
}