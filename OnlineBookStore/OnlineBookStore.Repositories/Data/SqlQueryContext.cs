using OnlineBookStore.Interfaces.Repository;

namespace OnlineBookStore.Repositories.Data
{
    public class SqlQueryContext : ISqlQueryContext
    {
        public string GetBooksByIds()
        {
            return $"SELECT b.Id, b.Title, b.Author, b.Price, b.Category, b.CreatedByADName, b.CreateDate, b.ModifyByADName, b.ModifyDate, b.Enabled FROM Books b WHERE b.Id IN (@Ids)";
        }
        public string GetCartItemsByShoppingCartId()
        {
            return @"SELECT ci.Id, ci.ShoppingCartId, ci.BookId, ci.Quantity, ci.CreatedByADName, ci.CreateDate, ci.ModifyByADName, ci.ModifyDate, ci.Enabled FROM CartItems ci WHERE ci.ShoppingCartId = @ShoppingCartId";
        }

        public string GetCartItemsByBookIdAndShoppingCartId()
        {
            return @"SELECT ci.Id, ci.ShoppingCartId, ci.BookId, ci.Quantity, ci.CreatedByADName, ci.CreateDate, ci.ModifyByADName, ci.ModifyDate, ci.Enabled FROM CartItems ci WHERE ci.BookId = @BookId AND ci.ShoppingCartId = @ShoppingCartId";
        }
    }
}
