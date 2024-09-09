namespace OnlineBookStore.Interfaces.Repository
{
    public interface ISqlQueryContext
    {
        string GetBooksByIds();

        string GetCartItemsByShoppingCartId();

        string GetCartItemsByBookIdAndShoppingCartId();
    }
}
