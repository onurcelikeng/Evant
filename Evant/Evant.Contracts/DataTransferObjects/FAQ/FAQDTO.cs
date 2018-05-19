using System;

namespace Evant.Contracts.DataTransferObjects.FAQ
{
    public sealed class FAQDTO
    {
        public Guid EventId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
