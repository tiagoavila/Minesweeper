using Minesweeper.Core.Enums;
using Minesweeper.Core.ValueObjects;
using System.Text;

namespace Minesweeper.Core.Entities
{
    public class Board
    {
        public Board(int boardSize, int numberOfBombs)
        {
            BoardSize = boardSize;
            NumberOfBombs = numberOfBombs;
            Cells = new Cell[boardSize, boardSize];
            CellBombs = new List<Cell>(numberOfBombs);
            NumberOfExposedCells = 0;
        }

        public int BoardSize { get; private set; }
        public int NumberOfBombs { get; private set; }
        public Cell[,] Cells { get; private set; }
        public ICollection<Cell> CellBombs { get; private set; }
        public int NumberOfExposedCells { get; private set; }
        public int NumberOfCells => BoardSize * BoardSize;

        public void InitializeBoard()
        {
            PlaceBombs();
            SetNumberedCells();
        }

        /// <summary>
        /// Instatiate the cells and place the bombs randomly
        /// </summary>
        private void PlaceBombs()
        {
            // Place bombs on the first N cells, where N is the value of the property NumberOfBombs
            for (int index = 0; index < NumberOfCells; index++)
            {
                int row = index / BoardSize;
                int column = (index - row * BoardSize) % BoardSize;

                Cell cell = new(row, column);
                if (index < NumberOfBombs)
                {
                    cell.SetIsBomb();
                    CellBombs.Add(cell);
                }

                Cells[row, column] = cell;
            }

            Random random = new();

            //Swap all the cells in the board
            for (int index1 = 0; index1 < NumberOfCells; index1++)
            {
                int index2 = index1 + random.Next(NumberOfCells - index1);
                if (index1 != index2)
                {
                    // Get cell at index1
                    int row1 = index1 / BoardSize;
                    int column1 = (index1 - row1 * BoardSize) % BoardSize;
                    Cell cell1 = Cells[row1, column1];

                    // Get cell at index2
                    int row2 = index2 / BoardSize;
                    int column2 = (index2 - row2 * BoardSize) % BoardSize;
                    Cell cell2 = Cells[row2, column2];

                    // Swap
                    Cells[row1, column1] = cell2 ?? new Cell(row1, column1);
                    cell2?.SetRowAndColumn(row1, column1);

                    Cells[row2, column2] = cell1;
                    cell1?.SetRowAndColumn(row2, column2);
                }
            }
        }

        /// <summary>
        /// Iterates through the bomb cells and sets the number of bombs in their neighbors
        /// </summary>
        private void SetNumberedCells()
        {
            foreach (var cellWithBomb in CellBombs)
            {
                var neighbors = cellWithBomb.GetNeighbors(BoardSize, Cells);
                foreach (var neighbor in neighbors)
                {
                    neighbor.IncreaseNumberOfSurroudingBombs();
                }
            }
        }

        public string GetBoardAsString()
        {
            StringBuilder cells = new();

            for (int row = 0; row < BoardSize; row++)
            {
                if (row != 0)
                {
                    cells.Append("| ");
                }

                for (int column = 0; column < BoardSize; column++)
                {
                    Cell cell = Cells[row, column];
                    cells.Append(cell.ToShortString());
                }
            }

            return cells.ToString();
        }

        /// <summary>
        /// Handles each play by seeing if the game is still valid to continue, if the player lost or won and flag a cell as a guess
        /// </summary>
        /// <param name="userPlay"></param>
        /// <returns></returns>
        public UserPlayResult Play(UserPlay userPlay)
        {
            Cell cell = Cells[userPlay.Row, userPlay.Column];

            if (userPlay.IsGuess)
            {
                cell.MarkAsGuess();
                return new UserPlayResult(true, GameStateEnum.StillAlive, new List<Cell>(0));
            }
            else
            {
                InclementNumberOfExposedCells(cell);
                cell.MarkAsExposed();
                var affectedCells = new List<Cell>(0);

                switch (cell.CellType)
                {
                    case CellTypeEnum.Bomb:
                        return new UserPlayResult(false, GameStateEnum.GameOver, new List<Cell>(0));

                    case CellTypeEnum.Blank:
                        ExpandBlankRegion(cell, out affectedCells);
                        break;
                }

                if (NumberOfExposedCells == NumberOfCells - NumberOfBombs)
                {
                    return new UserPlayResult(true, GameStateEnum.Won, affectedCells);
                }

                return new UserPlayResult(true, GameStateEnum.StillAlive, affectedCells);
            }
        }

        /// <summary>
        /// Expose blank cells until reach numbered cells
        /// </summary>
        /// <param name="cell"></param>
        public void ExpandBlankRegion(Cell cell, out List<Cell> affectedCells)
        {
            affectedCells = new List<Cell>();
            Queue<Cell> queueOfCellstoExplore = new();
            queueOfCellstoExplore.Enqueue(cell);

            while (queueOfCellstoExplore.Count > 0)
            {
                Cell current = queueOfCellstoExplore.Dequeue();

                foreach (var (rowDisplacement, columnDisplacement) in NeighborsPosition.NeighborsDisplacement)
                {
                    int neighborRow = current.Row + rowDisplacement;
                    int neighborColumn = current.Column + columnDisplacement;

                    if (Cell.IsNeighborInBounds(BoardSize, neighborRow, neighborColumn))
                    {
                        Cell neighbor = Cells[neighborRow, neighborColumn];
                        affectedCells.Add(neighbor);

                        //If is a blank cell adds it to the queue of cells to be expanded
                        if (neighbor.CellType == CellTypeEnum.Blank && !neighbor.IsExposed)
                        {
                            queueOfCellstoExplore.Enqueue(neighbor);
                        }

                        InclementNumberOfExposedCells(neighbor);
                        neighbor.MarkAsExposed();
                    }
                }
            }
        }

        public void ExposeAllCells()
        {
            for (int index = 0; index < NumberOfCells; index++)
            {
                int row = index / BoardSize;
                int column1 = (index - row * BoardSize) % BoardSize;
                Cell cell = Cells[row, column1];
                if (cell != null && !cell.IsExposed)
                {
                    cell.MarkAsExposed();
                }
            }
        }

        private void InclementNumberOfExposedCells(Cell cell)
        {
            if (cell.IsExposed)
                return;

            NumberOfExposedCells++;
        }
    }
}
