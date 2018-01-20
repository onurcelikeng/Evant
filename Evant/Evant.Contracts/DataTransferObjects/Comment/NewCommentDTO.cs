using System;

namespace Evant.Contracts.DataTransferObjects.Comment
{
    public sealed class NewCommentDTO
    {
        public Guid EventId { get; set; }

        public string Content { get; set; }
    }
}
