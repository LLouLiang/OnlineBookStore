using AutoMapper;
using Moq;
using OnlineBookStore.Interfaces;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Services;

namespace OnlineBookStore.Tests
{
    public class ShoppingCartServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartItemService> _cartItemServiceMock;
        private readonly Mock<IShoppingCartRepository> _shoppingCartRepositoryMock;
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartItemServiceMock = new Mock<ICartItemService>();
            _shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();

            // Setup UnitOfWork to return the ShoppingCartRepository
            _unitOfWorkMock.Setup(uow => uow.IShoppingCartRepository).Returns(_shoppingCartRepositoryMock.Object);

            // Create an instance of ShoppingCartService with the mocks
            _shoppingCartService = new ShoppingCartService(_mapperMock.Object, _unitOfWorkMock.Object, _cartItemServiceMock.Object);
        }

        [Fact]
        public async Task InsertShoppingCart_ShouldReturnSuccess_WhenShoppingCartIsAdded()
        {
            // Arrange
            var shoppingCartDto = new ShoppingCartDTO();
            var shoppingCart = new ShoppingCart();
            _mapperMock.Setup(m => m.Map<ShoppingCart>(shoppingCartDto)).Returns(shoppingCart);
            _shoppingCartRepositoryMock.Setup(repo => repo.InsertAsync(shoppingCart)).ReturnsAsync(1);

            // Act
            var result = await _shoppingCartService.InsertShoppingCart(shoppingCartDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Shopping cart added successfully", result.Message);
            Assert.Equal(1, shoppingCartDto.Id);
            _mapperMock.Verify(m => m.Map<ShoppingCart>(shoppingCartDto), Times.Once);
            _shoppingCartRepositoryMock.Verify(repo => repo.InsertAsync(shoppingCart), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task InsertShoppingCart_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            // Arrange
            var shoppingCartDto = new ShoppingCartDTO();
            _mapperMock.Setup(m => m.Map<ShoppingCart>(It.IsAny<ShoppingCartDTO>())).Throws(new Exception("Insert failed"));

            // Act
            var result = await _shoppingCartService.InsertShoppingCart(shoppingCartDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Insert failed", result.Message);
            _mapperMock.Verify(m => m.Map<ShoppingCart>(shoppingCartDto), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task GetShoppingCartById_ShouldReturnSuccess_WhenShoppingCartExists()
        {
            // Arrange
            var shoppingCart = new ShoppingCart { Id = 1 };
            var shoppingCartDto = new ShoppingCartDTO { Id = 1 };
            var cartItems = new List<CartItemDTO>();

            _shoppingCartRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(shoppingCart);
            _mapperMock.Setup(m => m.Map<ShoppingCartDTO>(shoppingCart)).Returns(shoppingCartDto);
            _cartItemServiceMock.Setup(ci => ci.GetCartItemsByShoppingCartId(1))
                .ReturnsAsync(new ServiceResponse<IEnumerable<CartItemDTO>>(true, "Success", "0001", "Success", cartItems));

            // Act
            var result = await _shoppingCartService.GetShoppingCartById(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Fetch shopping cart by Id successfully", result.Message);
            Assert.Equal(1, result.ResponseObject.Id);
            Assert.Equal(cartItems, result.ResponseObject.CartItems);
            _shoppingCartRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _cartItemServiceMock.Verify(ci => ci.GetCartItemsByShoppingCartId(1), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetShoppingCartById_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            // Arrange
            _shoppingCartRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ThrowsAsync(new Exception("Get failed"));

            // Act
            var result = await _shoppingCartService.GetShoppingCartById(1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Get failed", result.Message);
            _shoppingCartRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateShoppingCart_ShouldReturnSuccess_WhenShoppingCartIsUpdated()
        {
            // Arrange
            var shoppingCartDto = new ShoppingCartDTO { Id = 1 };
            var shoppingCart = new ShoppingCart { Id = 1 };

            _mapperMock.Setup(m => m.Map<ShoppingCart>(shoppingCartDto)).Returns(shoppingCart);

            // Act
            var result = await _shoppingCartService.UpdateShoppingCart(shoppingCartDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Update shopping cart successfully", result.Message);
            _mapperMock.Verify(m => m.Map<ShoppingCart>(shoppingCartDto), Times.Once);
            _shoppingCartRepositoryMock.Verify(repo => repo.UpdateAsync(shoppingCart), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddBookToShoppingCart_ShouldReturnSuccess_WhenBookIsAdded()
        {
            // Arrange
            var shoppingCartDto = new ShoppingCartDTO { Id = 1 };
            var cartItems = new List<CartItemDTO>();
            var cartItemDto = new CartItemDTO { ShoppingCartId = 1, BookId = 1, Quantity = 1 };

            _shoppingCartRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new ShoppingCart { Id = 1 });
            _mapperMock.Setup(m => m.Map<ShoppingCartDTO>(It.IsAny<ShoppingCart>())).Returns(shoppingCartDto);
            _cartItemServiceMock.Setup(ci => ci.GetCartItemsByShoppingCartId(1))
                .ReturnsAsync(new ServiceResponse<IEnumerable<CartItemDTO>>(true, "Success", "0001", "Success", cartItems));
            _cartItemServiceMock.Setup(ci => ci.AddCartItem(cartItemDto))
                .ReturnsAsync(new ServiceResponse<CartItemDTO>(true, "Item added", "0001", "Item added", cartItemDto));

            // Act
            var result = await _shoppingCartService.AddBookToShoppingCart(1, 1, 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Add book into shopping cart successfully", result.Message);
            _shoppingCartRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _cartItemServiceMock.Verify(ci => ci.AddCartItem(It.IsAny<CartItemDTO>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}
