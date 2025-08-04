using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendService _friendService;
        public FriendsController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost("add")]
        public IActionResult Add(Friend friend)
        {
            var result = _friendService.Add(friend);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(Friend friend)
        {
            var result = _friendService.Update(friend);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Friend friend)
        {
            var result = (_friendService.Delete(friend));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _friendService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _friendService.GetById(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
