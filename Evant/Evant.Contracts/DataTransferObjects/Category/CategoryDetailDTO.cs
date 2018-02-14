using System;

namespace Evant.Contracts.DataTransferObjects.Category
{
    public sealed class CategoryDetailDTO
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }
    }
}
