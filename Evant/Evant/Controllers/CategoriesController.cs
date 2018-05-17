using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Cognitive;
using Evant.Contracts.DataTransferObjects.Category;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Evant.Interfaces;
using Evant.Pay;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/categories")]
    public class CategoriesController : BaseController
    {
        private readonly IRepository<Category> _categoryRepo;
        private readonly ILogHelper _logHelper;


        public CategoriesController(IRepository<Category> categoryRepo,
            ILogHelper logHelper)
        {
            _categoryRepo = categoryRepo;
            _logHelper = logHelper;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var pay = new Iyzico();
            pay.test();

            try
            {
                var categories = (await _categoryRepo.All()).Select(c => new CategoryDetailDTO()
                {
                    CategoryId = c.Id,
                    Name = c.Name,
                    PhotoUrl = c.Icon
                }).ToList();

                if (categories.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logHelper.Log("CategoriesController", 500, "GetCategories", ex.Message);
                return null;
            }
        }

    }
}
