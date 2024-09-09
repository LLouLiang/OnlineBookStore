using AutoMapper;
using OnlineBookStore.Interfaces;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;

namespace OnlineBookStore.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IServiceResponse<BookDTO>> InsertBook(BookDTO bookDto)
        {
            try
            {
                var bookEntity = _mapper.Map<Book>(bookDto);

                var Id = await _unitOfWork.IBookRepository.InsertAsync(bookEntity);

                await _unitOfWork.CompleteAsync();

                var bookDtoResponse = _mapper.Map<BookDTO>(bookEntity);

                bookDtoResponse.Id = Id;

                return new ServiceResponse<BookDTO>(true, "Book added successfully", "0001", "Book added successfully", bookDtoResponse);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<BookDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<IEnumerable<BookDTO>>> GetAllBooks()
        {
            try
            {
                var books = await _unitOfWork.IBookRepository.GetAllAsync();

                await _unitOfWork.CompleteAsync();

                var bookDtos = _mapper.Map<IEnumerable<BookDTO>>(books);

                return new ServiceResponse<IEnumerable<BookDTO>>(true, "Books retrieved successfully", "0001", "Books retrieved successfully", bookDtos);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<BookDTO>>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<BookDTO>> GetBookById(long id)
        {
            try
            {
                var bookDB = await _unitOfWork.IBookRepository.GetByIdAsync(id);

                await _unitOfWork.CompleteAsync();

                var bookDto = _mapper.Map<BookDTO>(bookDB);

                return new ServiceResponse<BookDTO>(true, "Fetch book by Id successfully", "0001", "Fetch book by Id successfully", bookDto);
            }
            catch(Exception ex)
            {
                return new ServiceResponse<BookDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<IEnumerable<BookDTO>>> GetBooksByIds(List<long> ids)
        {
            try
            {
                using (_unitOfWork)
                {
                    var books = await _unitOfWork.IBookRepository.GetBooksByIds(ids);

                    var bookDtos = _mapper.Map<IEnumerable<BookDTO>>(books);

                    return new ServiceResponse<IEnumerable<BookDTO>>(true, "Fetch books by Ids successfully", "0001", "Fetch books by Ids successfully", bookDtos);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<BookDTO>> (false, ex.Message, "0002", ex.Message, null);
            }
        }
    }
}
