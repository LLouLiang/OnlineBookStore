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

        [HttpGet("{shoppingCartId}/total")]
        public async Task<IServiceResponse<string>> CalculateTotal(long shoppingCartId)
            => await _checkoutService.CalculateTotalAsync(shoppingCartId);
    }
}
