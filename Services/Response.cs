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

        public string Message { get; set; }
        public Error(string message)
        {
            Message = message;
        }
    }
}
