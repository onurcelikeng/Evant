using System;

namespace Evant.Contracts.DataTransferObjects.Business
{
    public sealed class AnnouncementDTO
    {
        public Guid EventId { get; set; }
        
        public string Message { get; set; }
    }
}
