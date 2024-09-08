namespace OnlineBookStore.Interfaces.Repository
{
    public interface ISqlQueryContext
    {
        string GetBooksByCategory(string categoryName);
    }
}
