using System;

namespace Evant.Contracts.DataTransferObjects.Comment
{
    public sealed class CommentDTO
    {
        public Guid EventId { get; set; }

        public string Content { get; set; }
    }
}
