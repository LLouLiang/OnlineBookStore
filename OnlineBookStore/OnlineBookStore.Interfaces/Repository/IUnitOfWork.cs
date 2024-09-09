namespace OnlineBookStore.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository IBookRepository { get; }
        IShoppingCartRepository IShoppingCartRepository { get; }
        ICartItemRepository ICartItemRepository { get; }
        Task<int> CompleteAsync();
    }
}
