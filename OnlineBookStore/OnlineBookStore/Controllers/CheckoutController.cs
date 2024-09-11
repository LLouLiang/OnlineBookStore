using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        /// <summary>
        /// Get: api/gettotalvalueofashoppingcart
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{shoppingCartId}/total")]
        public async Task<IServiceResponse<string>> CalculateTotal([FromRoute] long shoppingCartId)
            => await _checkoutService.CalculateTotalAsync(shoppingCartId);
    }
}
