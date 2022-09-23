using Minesweeper.Core.Enums;
using Minesweeper.Core.ValueObjects;
using SFML.Graphics;
using SFML.System;

namespace Minesweeper.Core
{
    public class Cell
    {
        public const int CELL_SIZE = 32;
        public const int RECTANGLE_SIZE = 32;

        public Cell(int row, int column, Texture? texture)
        {
            Row = row;
            Column = column;
            Texture = texture;
            CellKey = new Vector2f(Column, Row);
            Position = new Vector2f(Column * CELL_SIZE, Row * CELL_SIZE);
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
        public CellTypeEnum CellType => IsBomb ? CellTypeEnum.Bomb : NumberOfSurroudingBombs > 0 ? CellTypeEnum.Number : CellTypeEnum.Blank;
        public bool IsExposed { get; private set; }
        public bool IsGuess { get; private set; }
        public int NumberOfSurroudingBombs { get; private set; }
        public bool IsBomb { get; private set; }

        // ui
        public Vector2f CellKey { get; private set; }
        public Vector2f Position { get; private set; }
        public RectangleShape UIBox { get; private set; }
        public Texture Texture { get; private set; }
        public IntRect Rect { get; private set; }

        public void SetIsBomb()
        {
            IsBomb = true;
        }

        public void SetRowAndColumn(int newRow, int newColumn)
        {
            Row = newRow;
            Column = newColumn;

            CellKey = new Vector2f(Column, Row);
            Position = new Vector2f(Column * CELL_SIZE, Row * CELL_SIZE);
        }

        public void RenderUiBox()
        {
            UIBox = new RectangleShape(new Vector2f(CELL_SIZE, CELL_SIZE))
            {
                Texture = Texture,
                Position = Position,
                TextureRect = CreateTextureRect()
            };

            int leftPadding = 100; //This is the result of: GameConstants.WINDOW_WIDTH / 2 - GameConstants.BOARD_WIDTH / 2;
            int topPadding = 40; //This is the result of: GameConstants.WINDOW_HEIGHT / 2 - GameConstants.BOARD_HEIGHT / 2;

            Rect = new IntRect(leftPadding + (int)Position.X, topPadding + (int)Position.Y, CELL_SIZE, CELL_SIZE);
        }

        private IntRect CreateTextureRect()
        {
            (int left, int top) = GetLeftAndTopForUIRender();
            return new IntRect(left, top, RECTANGLE_SIZE, RECTANGLE_SIZE);
        }

        public void MarkAsExposed()
        {
            IsExposed = true;
            UIBox.TextureRect = CreateTextureRect();
        }

        public void MarkAsGuess()
        {
            IsGuess = !IsGuess;
            UIBox.TextureRect = CreateTextureRect();
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
        /// Get the pixels from the top and left of the Asset image gaming_SpriteSheet.png to render the correct image for the type of cell
        /// </summary>
        /// <returns></returns>
        private (int left, int top) GetLeftAndTopForUIRender()
        {
            if (!IsExposed && IsGuess)
                return (96, 64);

            if (!IsExposed)
                return (32, 64);

            int top = Math.DivRem(NumberOfSurroudingBombs, 4, out int left);

            return CellType switch
            {
                CellTypeEnum.Blank => (0, 0),
                CellTypeEnum.Number => (left * 32, top * 32),
                CellTypeEnum.Bomb => (64, 64),
                _ => (32, 64)
            };
        }

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