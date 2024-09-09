using AutoMapper;
using Moq;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Services;

namespace OnlineBookStore.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bookRepositoryMock = new Mock<IBookRepository>();

            // Set up the unit of work to return the mocked repository
            _unitOfWorkMock.Setup(uow => uow.IBookRepository).Returns(_bookRepositoryMock.Object);

            // Create an instance of BookService with mocks
            _bookService = new BookService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task InsertBook_ShouldReturnSuccess_WhenBookIsAdded()
        {
            // Arrange
            var bookDto = new BookDTO { Title = "Test Book", Author = "Author 1", Price = "9.99", Category = "Fiction", CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true };
            var book = new Book { Title = "Test Book", Author = "Author 1", Price = "9.99", Category = "Fiction", CreatedByADName = "llyu", CreateDate = DateTime.UtcNow, Enabled = true };
            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookDTO>())).Returns(book);
            _bookRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Book>())).ReturnsAsync(1);

            // Act
            var result = await _bookService.InsertBook(bookDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Book added successfully", result.Message);
            _mapperMock.Verify(m => m.Map<Book>(bookDto), Times.Once);
            _bookRepositoryMock.Verify(repo => repo.InsertAsync(book), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task InsertBook_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            // Arrange
            var bookDto = new BookDTO { Title = "Test Book" };
            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookDTO>())).Throws(new Exception("Insert failed"));

            // Act
            var result = await _bookService.InsertBook(bookDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Insert failed", result.Message);
            _mapperMock.Verify(m => m.Map<Book>(bookDto), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllBooks_ShouldReturnSuccess_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book> { new Book { Title = "Test Book" } };
            _bookRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDTO>>(books))
                       .Returns(new List<BookDTO> { new BookDTO { Title = "Test Book" } });

            // Act
            var result = await _bookService.GetAllBooks();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Books retrieved successfully", result.Message);
            _bookRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<BookDTO>>(books), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnSuccess_WhenBookExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            _bookRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDTO>(book)).Returns(new BookDTO { Id = 1, Title = "Test Book" });

            // Act
            var result = await _bookService.GetBookById(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Fetch book by Id successfully", result.Message);
            _bookRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mapperMock.Verify(m => m.Map<BookDTO>(book), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBooksByIds_ShouldReturnSuccess_WhenBooksExist()
        {
            // Arrange
            var ids = new List<long> { 1, 2 };
            var books = new List<Book> { new Book { Id = 1 }, new Book { Id = 2 } };
            _bookRepositoryMock.Setup(repo => repo.GetBooksByIds(ids)).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDTO>>(books))
                       .Returns(new List<BookDTO> { new BookDTO { Id = 1 }, new BookDTO { Id = 2 } });

            // Act
            var result = await _bookService.GetBooksByIds(ids);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Fetch books by Ids successfully", result.Message);
            _bookRepositoryMock.Verify(repo => repo.GetBooksByIds(ids), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<BookDTO>>(books), Times.Once);
        }
    }
}
