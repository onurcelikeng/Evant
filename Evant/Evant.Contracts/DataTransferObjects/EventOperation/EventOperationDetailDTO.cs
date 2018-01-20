using Evant.Contracts.DataTransferObjects.User;
using System;

namespace Evant.Contracts.DataTransferObjects.EventOperation
{
    public sealed class EventOperationDetailDTO
    {
        public Guid EventOperationId { get; set; }

        public UserInfoDTO User { get; set; }
    }
}
