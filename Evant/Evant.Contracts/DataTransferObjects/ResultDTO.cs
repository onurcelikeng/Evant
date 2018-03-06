namespace Evant.Contracts.DataTransferObjects
{
    public class ResultDTO<T> where T : class
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public T Data { get; set; }
    }
}
