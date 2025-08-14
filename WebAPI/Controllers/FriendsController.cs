using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("me")]
        [Authorize(Roles = "Member,Admin")]
        public IActionResult GetMyFriends()
        {
            // 1) JWT içinden giriş yapan kullanıcının Id bilgisini çekiyoruz.
            //    ClaimTypes.NameIdentifier → bizim JWT oluştururken eklediğimiz "user id" bilgisi.
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // 2) userIdClaim string olduğu için int'e çeviriyoruz.
            //    Çevirme başarısız olursa (null veya sayı değilse) Unauthorized dönüyoruz.
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            // 3) Servis katmanındaki GetFriendsOfUser metodunu çağırıyoruz.
            //    Burada Business katmanı, DAL'den join'li arkadaş listesini çekecek.
            var result = _friendService.GetFriendsWithUserInfo(userId);

            // 4) Eğer işlem başarılı ise 200 OK ve data dönüyoruz, başarısızsa 400 BadRequest.
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
