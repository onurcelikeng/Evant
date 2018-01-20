namespace Evant.Contracts.DataTransferObjects.Account
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ReNewPassword { get; set; }
    }
}