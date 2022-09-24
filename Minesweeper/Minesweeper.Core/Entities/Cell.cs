using Minesweeper.Core.Enums;
using Minesweeper.Core.ValueObjects;

namespace Minesweeper.Core
{
    public class Cell
    {
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
        public CellTypeEnum CellType => IsBomb ? CellTypeEnum.Bomb : NumberOfSurroudingBombs > 0 ? CellTypeEnum.Number : CellTypeEnum.Blank;
        public bool IsExposed { get; private set; }
        public bool IsGuess { get; private set; }
        public int NumberOfSurroudingBombs { get; private set; }
        public bool IsBomb { get; private set; }

        public void SetIsBomb()
        {
            IsBomb = true;
        }

        public void SetRowAndColumn(int newRow, int newColumn)
        {
            Row = newRow;
            Column = newColumn;
        }

        public void MarkAsExposed()
        {
            IsExposed = true;
        }

        public void MarkAsGuess()
        {
            IsGuess = !IsGuess;
        }

        public void IncreaseNumberOfSurroudingBombs()
        {
            NumberOfSurroudingBombs += 1;
        }

        public IEnumerable<Cell> GetNeighbors(int boardSize, Cell[,] cells)
        {
            List<Cell> neighbors = new(NeighborsPosition.NeighborsDisplacement.Length);

            foreach (var (rowDisplacement, columnDisplacement) in NeighborsPosition.NeighborsDisplacement)
            {
                int neighborRow = Row + rowDisplacement;
                int neighborColumn = Column + columnDisplacement;

                if (IsNeighborInBounds(boardSize, neighborRow, neighborColumn))
                {
                    neighbors.Add(cells[neighborRow, neighborColumn]);
                }
            }

            return neighbors;
        }

        public string ToShortString() => CellType switch
        {
            CellTypeEnum.Blank => "-",
            CellTypeEnum.Number => NumberOfSurroudingBombs.ToString(),
            CellTypeEnum.Bomb => "*",
            _ => string.Empty
        };

        /// <summary>
        /// Check if a neighbor of a cell is inside the bounds of the 2D array
        /// </summary>
        /// <param name="boardSize"></param>
        /// <param name="neighborRow"></param>
        /// <param name="neighborColumn"></param>
        /// <returns></returns>
        public static bool IsNeighborInBounds(int boardSize, int neighborRow, int neighborColumn)
        {
            return neighborRow >= 0 && neighborRow < boardSize && neighborColumn >= 0 && neighborColumn < boardSize;
        }
    }
}