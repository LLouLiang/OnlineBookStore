using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// POST: api/shoppingcart/add/{bookId}/{quantity}
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost("add/{bookId}/{quantity}")]
        public async Task<IActionResult> AddToCart(int bookId, int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            await _shoppingCartService.AddToCartAsync(bookId, quantity);
            return Ok();
        }

        /// <summary>
        /// GET: api/shoppingcart
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await _shoppingCartService.GetCartItemsAsync();
            return Ok(cartItems);
        }

        /// <summary>
        /// DELETE: api/shoppingcart/clear
        /// </summary>
        /// <returns></returns>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            await _shoppingCartService.ClearCartAsync();
            return Ok();
        }
    }
}
