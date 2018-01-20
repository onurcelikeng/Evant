namespace Evant.Contracts.DataTransferObjects.Account
{
    public class TokenResultDTO
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Token { get; set; }
    }
}