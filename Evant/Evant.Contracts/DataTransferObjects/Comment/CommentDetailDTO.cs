using System;
using Evant.Contracts.DataTransferObjects.User;

namespace Evant.Contracts.DataTransferObjects.Comment
{
    public sealed class CommentDetailDTO
    {
        public Guid CommentId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserInfoDTO User { get; set; }
    }
}
