using Evant.Contracts.DataTransferObjects.Business;
using Evant.Contracts.DataTransferObjects.UserSettingDTO;

namespace Evant.Contracts.DataTransferObjects.User
{
    public sealed class UserDetailDTO : BaseUserDetailDTO
    {
        public bool IsBusiness { get; set; }

        public UserSettingInfoDTO Settings { get; set; }

        public BusinessInfoDTO Business { get; set; }
    }
}
