namespace lewBlazorServer.Services
{
    public class Response<T>
    {
        public bool Ok { get => Errors.Count == 0; }
        public List<Error> Errors { get; set; } = new();
        public T Data { get; set; }
    }

    public class Error
    {
        public ErrorCode Code { get; set; } = ErrorCode.NotSet;
        public string Message { get; set; }
        public Error(string message) : this(ErrorCode.NotSet, message)
        {
        }

        public Error(ErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public enum ErrorCode
    {
        NotSet,
        NotFound,
        AlreadyExist
    }
}
