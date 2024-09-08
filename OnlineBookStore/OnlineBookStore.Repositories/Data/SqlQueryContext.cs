using OnlineBookStore.Interfaces.Repository;

namespace OnlineBookStore.Repositories.Data
{
    public class SqlQueryContext : ISqlQueryContext
    {
        public string GetBooksByCategory(string categoryName)
        {
            return @"SELECT b.BookId, b.ISBN, b.Name, b.Author, b.Price, b.Category FROM Book b WHERE b.Category = @categoryName";
        }
    }
}
