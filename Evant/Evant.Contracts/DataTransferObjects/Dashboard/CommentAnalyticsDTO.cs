using System;
using System.Collections.Generic;

namespace Evant.Contracts.DataTransferObjects.Dashboard
{
    public sealed class CommentAnalyticsDTO
    {
        public Guid CommentId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Language { get; set; }

        public string LanguageCode { get; set; }

        public string Sentiment { get; set; }

        public List<string> KeyPhrases { get; set; }
    }
}
