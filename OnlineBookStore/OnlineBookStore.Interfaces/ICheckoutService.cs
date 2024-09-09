namespace OnlineBookStore.Interfaces
{
    public interface ICheckoutService
    {
        Task<IServiceResponse<string>> CalculateTotalAsync(long shoppingCartId);
    }
}
