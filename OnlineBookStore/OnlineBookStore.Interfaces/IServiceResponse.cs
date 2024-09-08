namespace OnlineBookStore.Interfaces
{
    public interface IServiceResponse<T> : IServiceResponse
    {
        T ResponseObject { get; set; }
    }
    public interface IServiceResponse
    {
        bool Success { get; }
        string Message { get; }
        string MessageCode { get; }
        string LocalizedMessage { get; }
        string Warning { get; }
    }
}
