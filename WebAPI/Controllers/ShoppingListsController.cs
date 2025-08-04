using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        public ShoppingListsController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpPost("add")]
        public IActionResult Add(ShoppingList shoppingList)
        {
            var result = _shoppingListService.Add(shoppingList);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(ShoppingList shoppingList)
        {
            var result = _shoppingListService.Update(shoppingList);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(ShoppingList shoppingList)
        {
            var result = (_shoppingListService.Delete(shoppingList));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _shoppingListService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _shoppingListService.GetById(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
