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

        [HttpGet("total")]
        public async Task<IActionResult> CalculateTotal()
        {
            var totalPrice = await _checkoutService.CalculateTotalAsync();
            return Ok(totalPrice);
        }
    }
}
