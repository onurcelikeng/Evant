using Evant.Contracts.DataTransferObjects.UserSettingDTO;

namespace Evant.Contracts.DataTransferObjects.User
{
    public sealed class UserDetailDTO : BaseUserDetailDTO
    {
        public UserSettingInfoDTO Settings { get; set; }
    }
}
