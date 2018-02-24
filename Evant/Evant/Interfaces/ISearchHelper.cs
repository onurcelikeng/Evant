using System;
using System.Threading.Tasks;

namespace Evant.Interfaces
{
    public interface ISearchHelper
    {
        Task<bool> Add(Guid userId, string keyword);
    }
}
