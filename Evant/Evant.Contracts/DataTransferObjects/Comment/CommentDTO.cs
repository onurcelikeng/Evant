using System;
using System.Collections.Generic;
using System.Text;

namespace Evant.Contracts.DataTransferObjects.Comment
{
    public class CommentDTO : BaseDTO
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
