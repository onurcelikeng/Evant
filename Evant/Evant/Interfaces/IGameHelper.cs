using System;
using System.Threading.Tasks;
using static Evant.Constants.GameConstant;

namespace Evant.Interfaces
{
    public interface IGameHelper
    {
        Task Point(Guid userId, GameType type);
    }
}
