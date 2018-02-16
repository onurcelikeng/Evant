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

    }
}
