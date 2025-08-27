using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("add")]
        public IActionResult Add(Notification notification)
        {
            var result = _notificationService.Add(notification);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(Notification notification)
        {
            var result = _notificationService.Update(notification);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Notification notification)
        {
            var result = (_notificationService.Delete(notification));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _notificationService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _notificationService.GetById(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Benim bildirimlerim (opsiyonel filtre: isRead)
        [HttpGet("me")]
        public IActionResult GetMyNotifications(bool? isRead, int page = 1, int pageSize = 20)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _notificationService.GetMyNotifications(me, isRead, page, pageSize);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("me/count")]
        public IActionResult GetMyNotificationsCount(bool? isRead)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _notificationService.GetMyNotificationsCount(me, isRead);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id:int}/read")]
        public IActionResult MarkAsRead(int id)
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _notificationService.MarkAsRead(id, me);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("read-all")]
        public IActionResult MarkAllAsRead()
        {
            var meStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(meStr, out var me)) return Unauthorized();

            var result = _notificationService.MarkAllAsRead(me);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
