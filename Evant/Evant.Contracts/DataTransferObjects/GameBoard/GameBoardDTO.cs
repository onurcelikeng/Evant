using Evant.Contracts.DataTransferObjects.User;

namespace Evant.Contracts.DataTransferObjects.GameBoard
{
    public sealed class GameBoardDTO : UserInfoDTO
    {
        public int Score { get; set; } = 0;
    }
}
