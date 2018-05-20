using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.FAQ;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/faqs")]
    public class FAQController : BaseController
    {
        private readonly IRepository<FAQ> _faqRepo;
        private readonly ILogHelper _logHelper;


        public FAQController(IRepository<FAQ> faqRepo,
            ILogHelper logHelper)
        {
            _faqRepo = faqRepo;
            _logHelper = logHelper;
        }


        [HttpGet("{eventId}")]
        public async Task<IActionResult> FAQs([FromRoute] Guid eventId)
        {
            try
            {
                var faqs = (await _faqRepo.Where(f => f.EventId == eventId)).Select(s => new FAQInfoDTO()
                {
                    FAQId = s.Id,
                    Question = s.Question,
                    Answer = s.Answer,
                    CreateAt = s.CreatedAt
                }).ToList();

                if (faqs.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(faqs);
            }
            catch (Exception ex)
            {
                _logHelper.Log("FAQController", 500, "GetFAQs", ex.Message);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFAQ([FromBody] FAQDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                var faq = new FAQ()
                {
                    EventId = model.EventId,
                    Question = model.Question,
                    Answer = model.Answer
                };

                var response = await _faqRepo.Add(faq);
                if (response)
                    return Ok("FAQ eklendi");
                else
                    return BadRequest("FAQ eklenemedi.");
            }
            catch (Exception ex)
            {
                _logHelper.Log("FAQController", 500, "AddFAQ", ex.Message);
                return null;
            }
        }

        [HttpDelete("{faqId}")]
        public async Task<IActionResult> DeleteFAQ([FromRoute] Guid faqId)
        {
            try
            {
                var faq = await _faqRepo.First(f => f.Id == faqId);
                if (faq == null)
                    return NotFound("Kayıt bulunamadı.");

                var response = await _faqRepo.Delete(faq);
                if (response)
                    return Ok("FAQ silindi");
                else
                    return BadRequest("FAQ silinemedi.");
            }
            catch (Exception ex)
            {
                _logHelper.Log("FAQController", 500, "DeleteFAQ", ex.Message);
                return null;
            }
        }

    }
}
