using System;

namespace Evant.Contracts.DataTransferObjects.Category
{
    public sealed class CategoryInfoDTO
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string IconUrl { get; set; }
    }
}
