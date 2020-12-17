using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetClinicWebApi.Model;
using PetClinicWebApi.Services;

namespace PetClinicWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageBoardsController : ControllerBase
    {
        private readonly QAService _qAService;
        private readonly MessageBoardService _messageBoardService;
        public MessageBoardsController(QAService qAService, MessageBoardService messageBoardService)
        {
            _qAService = qAService;
            _messageBoardService = messageBoardService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QA>>> GetAllQA()
        {
            var qAs = await _qAService.GetAll();
            return Ok(qAs);
        }
        [HttpPost]
        public async Task<IActionResult> CreateQA(QA qA)
        {
            await _qAService.CreateAsync(qA);
            return Ok(qA);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQA(int index, QA model)
        {
            var qAs = await _qAService.GetAll();
            var qA = qAs[index];
            if (qA == null)
            {
                return NotFound();
            }
            await _qAService.UpdateAsync(qA.QAId, model);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteQA(int index)
        {
            var qAs = await _qAService.GetAll();
            var qA = qAs[index];
            if (qA == null)
            {
                return NotFound();
            }
            await _qAService.DeleteAsync(qA.QAId);
            return NoContent();
        }

        public async Task<IActionResult> CreateMessage(MessageBoard message)
        {
            message.CreatedTime = DateTime.Now.Date;
            message.UserName = HttpContext.User.Identity.Name;
            await _messageBoardService.CreateAsync(message);
            return Ok(message);
        }
        
        
    }
}
