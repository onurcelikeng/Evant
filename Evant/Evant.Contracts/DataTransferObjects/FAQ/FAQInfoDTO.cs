using System;

namespace Evant.Contracts.DataTransferObjects.FAQ
{
    public class FAQInfoDTO
    {
        public Guid FAQId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
