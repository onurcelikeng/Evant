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
    [Route("api/categories")]
    public class CategoriesController : BaseController
    {
        private readonly IRepository<Category> _categoryRepo;


        public CategoriesController(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }


        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepo.GetAll().Select(c => new CategoryDetailDTO()
            {
                CategoryId = c.Id,
                Name = c.Name,
                PhotoUrl = c.Icon
            }).ToList();

            if (categories.IsNullOrEmpty())
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                return Ok(categories);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory([FromRoute] Guid id)
        {
            var category = _categoryRepo.First(c => c.Id == id);
            if (category == null)
            {
                return NotFound("Kayıt bulunamadı.");
            }

            return Ok(category);
        }

        [HttpPut]
        public IActionResult AddCategory([FromBody] CategoryDetailDTO category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(null);
            }

            var selectedCategory = _categoryRepo.First(c => c.Id == category.CategoryId);
            if (selectedCategory == null)
            {
                return NotFound("Kategori bulunamadı.");
            }
            else
            {
                selectedCategory.Name = category.Name;
                selectedCategory.Icon = category.PhotoUrl;
                selectedCategory.UpdateAt = DateTime.Now;

                var response = _categoryRepo.Update(selectedCategory);
                if (response)
                {
                    return Ok("Kategori güncellendi.");
                }
                else
                {
                    return BadRequest("Kategori güncellenemedi.");
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateCategory([FromBody] CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(null);
            }

            var response = _categoryRepo.Insert(new Category()
            {
                Id = new Guid(),
                Name = category.Name,
                Icon = category.PhotoUrl,
                CreatedAt = DateTime.Now
            });
            if (response)
            {
                return Ok("Kategori eklendi.");
            }
            else
            {
                return BadRequest("Kategori eklenemedi.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory([FromRoute] Guid id)
        {
            var category = _categoryRepo.First(c => c.Id == id);
            if (category == null)
            {
                return NotFound("Böyle bir kategori yok.");
            }

            var response = _categoryRepo.Delete(category);
            if (response)
            {
                return Ok("Kategori silindi.");
            }
            else
            {
                return BadRequest("Kategori silinemedi.");
            }
        }

    }
}
