using OnlineBookStore.Interfaces;

namespace OnlineBookStore.Services
{
    [Serializable]
    public class ServiceResponse<T> : IServiceResponse<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public string MessageCode { get; private set; }
        public string LocalizedMessage { get; private set; }
        public T ResponseObject { get; set; }
        public string Warning { get; private set; }
        public ServiceResponse(bool success, string message, string messageCode, string localizedMessage, T responseObject)
        {
            Success = success;
            MessageCode = messageCode;
            LocalizedMessage = localizedMessage;
            Message = message;
            ResponseObject = responseObject;
        }
    }
}
