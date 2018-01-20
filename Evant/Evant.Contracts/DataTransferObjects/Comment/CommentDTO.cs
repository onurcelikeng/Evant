using Evant.Contracts.DataTransferObjects.User;
using System;

namespace Evant.Contracts.DataTransferObjects.Comment
{
    public sealed class CommentDTO
    {
        public Guid CommentId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserInfoDTO User { get; set; }
    }
}
