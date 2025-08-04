using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketsController : ControllerBase
    {
        private readonly IMarketService _marketService;
        public MarketsController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [HttpPost("add")]
        public IActionResult Add(Market market)
        {
            var result = _marketService.Add(market);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(Market market)
        {
            var result = _marketService.Update(market);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Market market)
        {
            var result = (_marketService.Delete(market));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _marketService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _marketService.GetById(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
