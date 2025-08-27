using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("add")]
        public IActionResult Add(Message message)
        {
            var result = _messageService.Add(message);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(Message message)
        {
            var result = _messageService.Update(message);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _messageService.Delete(new Message { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _messageService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _messageService.GetById(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Sohbet geçmişi: /api/messages/conversation/{otherUserId}?page=1&pageSize=20
        [HttpGet("conversation/{otherUserId:int}")]
        public IActionResult GetConversation(int otherUserId, int page = 1, int pageSize = 20)
        {
            // Token'dan me (giriş yapan) çek
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _messageService.GetConversation(me, otherUserId, page, pageSize);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Toplam kayıt sayısı (UI'da sayfalama için)
        [HttpGet("conversation/{otherUserId:int}/count")]
        public IActionResult GetConversationCount(int otherUserId)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _messageService.GetConversationCount(me, otherUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Sohbet arama (ben <-> otherUserId)
        // /api/messages/conversation/{otherUserId}/search?keyword=...&from=2025-08-01&to=2025-08-27&page=1&pageSize=20
        [HttpGet("conversation/{otherUserId:int}/search")]
        public IActionResult SearchConversation(int otherUserId, string? keyword, DateTime? from, DateTime? to, int page = 1, int pageSize = 20)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _messageService.SearchConversation(me, otherUserId, keyword, from, to, page, pageSize);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("conversation/{otherUserId:int}/search/count")]
        public IActionResult SearchConversationCount(int otherUserId, DateTime? from, DateTime? to)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _messageService.SearchConversationCount(me, otherUserId, from, to);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Benim tüm mesajlarımda arama (inbox+sent)
        // /api/messages/search?keyword=...&from=2025-08-01&to=2025-08-27&page=1&pageSize=20
        [HttpGet("search")]
        public IActionResult SearchMyMessages(string? keyword, DateTime? from, DateTime? to, int page = 1, int pageSize = 20)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _messageService.SearchMyMessages(me, keyword, from, to, page, pageSize);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("search/count")]
        public IActionResult SearchMyMessagesCount(DateTime? from, DateTime? to)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _messageService.SearchMyMessagesCount(me, from, to);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
