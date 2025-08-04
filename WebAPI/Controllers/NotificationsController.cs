using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
