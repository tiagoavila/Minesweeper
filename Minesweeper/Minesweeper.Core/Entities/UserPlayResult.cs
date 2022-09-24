using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Entities
{
    public class UserPlayResult
    {
        public UserPlayResult(bool isSuccessful, GameStateEnum resultingGameState, List<Cell> affectedCells)
        {
            IsSuccessful = isSuccessful;
            ResultingGameState = resultingGameState;
            AffectedCells = affectedCells;
        }

        public bool IsSuccessful { get; private set; }
        public GameStateEnum ResultingGameState { get; private set; }
        public List<Cell> AffectedCells { get; set; }
    }
}
