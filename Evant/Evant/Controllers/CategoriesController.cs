using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects.Category;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IRepository<Category> _categoryRepo;


        public CategoriesController(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }


        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepo.GetAll().Select(c => new CategoryDTO()
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon
            });

            return BaseController.Instance.Result(categories, 200);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory([FromRoute] Guid id)
        {
            var category = _categoryRepo.First(c => c.Id == id);
            if (category == null)
            {
                return BaseController.Instance.Result(null, 404, "Böyle bir kategori yok.");
            }

            return BaseController.Instance.Result(category, 200);
        }

        [HttpPut]
        public IActionResult PutCategory([FromBody] CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return BaseController.Instance.Result(null, 400);
            }

            var selectedCategory = _categoryRepo.First(c => c.Id == category.Id);
            if (selectedCategory == null)
            {
                return BaseController.Instance.Result(null, 404, "Kategori bulunamadı.");
            }
            else
            {
                selectedCategory.Name = category.Name;
                selectedCategory.Icon = category.Icon;
                selectedCategory.UpdateAt = DateTime.Now;

                var response = _categoryRepo.Update(selectedCategory);
                if (response)
                {
                    return BaseController.Instance.Result(null, 200, "Kategori güncellendi.");
                }
                else
                {
                    return BaseController.Instance.Result(null, 500, "Kategori güncellenemedi.");
                }
            }
        }

        [HttpPost]
        public IActionResult PostCategory([FromBody] CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return BaseController.Instance.Result(null, 400);
            }

            var response = _categoryRepo.Insert(new Category()
            {
                Id = new Guid(),
                Name = category.Name,
                Icon = category.Icon,
                CreatedAt = DateTime.Now
            });
            if (response)
            {
                return BaseController.Instance.Result(null, 200, "Kategori eklendi.");
            }
            else
            {
                return BaseController.Instance.Result(null, 500, "Kategori eklenemedi.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory([FromRoute] Guid id)
        {
            var category = _categoryRepo.First(c => c.Id == id);
            if (category == null)
            {
                return BaseController.Instance.Result(null, 404, "Böyle bir kategori yok.");
            }

            var response = _categoryRepo.Delete(category);
            if (response)
            {
                return BaseController.Instance.Result(null, 200, "Kategori silindi.");
            }
            else
            {
                return BaseController.Instance.Result(null, 500, "Kategori silinemedi.");
            }
        }

    }
}
