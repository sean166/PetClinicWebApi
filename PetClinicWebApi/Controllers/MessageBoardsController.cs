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
        
        public async Task<IActionResult> AddComment(MessageBoard comment, string Qid)
        {
            if (ModelState.IsValid == true)
            {
                comment.CreatedTime = DateTime.Now.Date;
                await _messageBoardService.CreateAsync(comment);

                var question = await _messageBoardService.GetMessage(Qid);
                var a = question.RepliedMessages;
                a = (List<string>)a.Append(comment.MessageBoardId.ToString());
                question.RepliedMessages = a;
                await _messageBoardService.UpdateAsync(question.MessageBoardId, question);
                return Ok(comment);
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");


        }
        public async Task<IActionResult> UpdateLikes(string id, bool tag)
        {
            var message = await _messageBoardService.GetMessage(id);
            if (tag == true)
            {
                message.Likes += 1;
                await _messageBoardService.UpdateAsync(id, message);
            }
            return Ok(message);
        }
        public async Task<IActionResult> ListAllMessages()
        {
            //github is a trash
            var messageList = await _messageBoardService.GetAll();
            return Ok(messageList);
        }

        //public async Task<MessageBoard> DisplayAllAsync(MessageBoard m)
        //{
        //    var messageList = new List<MessageBoard>();

        //    if (m.RepliedMessages != null)
        //    {
        //        foreach (var r in m.RepliedMessages)
        //        {
        //            var reply = await _messageBoardService.GetMessage(r);


        //        }
        //    }

        
        //}
           
          
        
        
    }
}
