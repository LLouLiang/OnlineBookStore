using Moq;
using OnlineBookStore.Interfaces;
using OnlineBookStore.Models;
using OnlineBookStore.Services;

namespace OnlineBookStore.Tests
{
    public class CheckoutServiceTests
    {
        private readonly Mock<IShoppingCartService> _shoppingCartServiceMock;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly CheckoutService _checkoutService;

        public CheckoutServiceTests()
        {
            _shoppingCartServiceMock = new Mock<IShoppingCartService>();
            _bookServiceMock = new Mock<IBookService>();
            _checkoutService = new CheckoutService(_shoppingCartServiceMock.Object, _bookServiceMock.Object);
        }

        [Fact]
        public async Task CalculateTotalAsync_ShouldReturnFailure_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            _shoppingCartServiceMock.Setup(s => s.GetShoppingCartById(It.IsAny<long>()))
                .ReturnsAsync(new ServiceResponse<ShoppingCartDTO>(false, "Shopping cart does not exist", "0002", "Shopping cart does not exist", null));

            // Act
            var result = await _checkoutService.CalculateTotalAsync(1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Shopping cart does not exist", result.Message);
        }

        [Fact]
        public async Task CalculateTotalAsync_ShouldReturnSuccess_WhenCartItemsAreEmpty()
        {
            // Arrange
            var shoppingCart = new ShoppingCartDTO { CartItems = null };
            _shoppingCartServiceMock.Setup(s => s.GetShoppingCartById(It.IsAny<long>()))
                .ReturnsAsync(new ServiceResponse<ShoppingCartDTO>(true, "Shopping cart retrieved", "0001", "Shopping cart retrieved", shoppingCart));

            // Act
            var result = await _checkoutService.CalculateTotalAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Get shopping cart items calculated successfully", result.Message);
            Assert.Equal("0", result.ResponseObject);
        }

        [Fact]
        public async Task CalculateTotalAsync_ShouldReturnSuccess_WhenCartItemsExist()
        {
            // Arrange
            var cartItems = new List<CartItemDTO>
            {
                new CartItemDTO { BookId = 1, Quantity = 2 },
                new CartItemDTO { BookId = 2, Quantity = 3 }
            };

            var shoppingCart = new ShoppingCartDTO { CartItems = cartItems };
            var books = new List<BookDTO>
            {
                new BookDTO { Id = 1, Price = "10.00" },
                new BookDTO { Id = 2, Price = "20.00" }
            };

            _shoppingCartServiceMock.Setup(s => s.GetShoppingCartById(It.IsAny<long>()))
                .ReturnsAsync(new ServiceResponse<ShoppingCartDTO>(true, "Shopping cart retrieved", "0001", "Shopping cart retrieved", shoppingCart));

            _bookServiceMock.Setup(b => b.GetBooksByIds(It.IsAny<List<long>>()))
                .ReturnsAsync(new ServiceResponse<IEnumerable<BookDTO>>(true, "Books retrieved", "0001", "Books retrieved", books));

            // Act
            var result = await _checkoutService.CalculateTotalAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Get shopping cart items calculated successfully", result.Message);
            Assert.Equal("80.00", result.ResponseObject);
        }

        [Fact]
        public async Task CalculateTotalAsync_ShouldHandleMissingBookPriceGracefully()
        {
            // Arrange
            var cartItems = new List<CartItemDTO>
            {
                new CartItemDTO { BookId = 1, Quantity = 2 },
                new CartItemDTO { BookId = 2, Quantity = 3 }
            };

            var shoppingCart = new ShoppingCartDTO { CartItems = cartItems };
            var books = new List<BookDTO>
            {
                new BookDTO { Id = 1, Price = "10.00" },
                new BookDTO { Id = 2, Price = null }
            };

            _shoppingCartServiceMock.Setup(s => s.GetShoppingCartById(It.IsAny<long>()))
                .ReturnsAsync(new ServiceResponse<ShoppingCartDTO>(true, "Shopping cart retrieved", "0001", "Shopping cart retrieved", shoppingCart));

            _bookServiceMock.Setup(b => b.GetBooksByIds(It.IsAny<List<long>>()))
                .ReturnsAsync(new ServiceResponse<IEnumerable<BookDTO>>(true, "Books retrieved", "0001", "Books retrieved", books));

            // Act
            var result = await _checkoutService.CalculateTotalAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Get shopping cart items calculated successfully", result.Message);
            Assert.Equal("20.00", result.ResponseObject);
        }
    }
}
