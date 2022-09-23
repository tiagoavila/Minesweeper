namespace Minesweeper.Core.Entities
{
    public class UserPlay
    {
        public UserPlay(int row, int column, bool isGuess)
        {
            Row = row;
            Column = column;
            IsGuess = isGuess;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
        public bool IsGuess { get; private set; }
    }
}
