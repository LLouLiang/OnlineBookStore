using Microsoft.AspNetCore.Authorization;
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
        /// Get: api/addbooktoshoppingcart
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <param name="bookId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add/{shoppingCartId}/{bookId}/{quantity}")]
        public async Task<IServiceResponse<ShoppingCartDTO>> AddBookToShoppingCart([FromRoute] long shoppingCartId, [FromRoute] long bookId, [FromRoute] int quantity)
            => await _shoppingCartService.AddBookToShoppingCart(shoppingCartId, bookId, quantity);

        /// <summary>
        /// GET: api/getshoppingcartbyid
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IServiceResponse<ShoppingCartDTO>> GetCartItems([FromRoute] long id)
            => await _shoppingCartService.GetShoppingCartById(id);
    }
}
