using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Entities
{
    public class UserPlayResult
    {
        public UserPlayResult(bool isSuccessful, GameStateEnum resultingGameState)
        {
            IsSuccessful = isSuccessful;
            ResultingGameState = resultingGameState;
        }

        public bool IsSuccessful { get; private set; }
        public GameStateEnum ResultingGameState { get; private set; }
    }
}
