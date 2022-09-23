using Minesweeper.Core.Enums;
using Minesweeper.Core.ValueObjects;
using SFML.Graphics;
using SFML.System;

namespace Minesweeper.Core
{
    public class Cell
    {
        public const int CELL_SIZE = 16;
        public const int RECTANGLE_SIZE = 10;

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
            Rect = new IntRect(160 + (int)Position.X, 80 + (int)Position.Y, CELL_SIZE, CELL_SIZE);
        }

        private IntRect CreateTextureRect()
        {
            return new IntRect(GetCellNumberForUIRender() * 10, 0, RECTANGLE_SIZE, RECTANGLE_SIZE);
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

        public IEnumerable<Cell> GetNeighboors(int boardSize, Cell[,] cells)
        {
            List<Cell> neighboors = new(NeighboorsPosition.NeighboorsDisplacement.Length);

            foreach (var (rowDisplacement, columnDisplacement) in NeighboorsPosition.NeighboorsDisplacement)
            {
                int neighboorRow = Row + rowDisplacement;
                int neighboorColumn = Column + columnDisplacement;

                if (IsNeighboorInBounds(boardSize, neighboorRow, neighboorColumn))
                {
                    neighboors.Add(cells[neighboorRow, neighboorColumn]);
                }
            }

            return neighboors;
        }

        public string ToShortString() => CellType switch
        {
            CellTypeEnum.Blank => "-",
            CellTypeEnum.Number => NumberOfSurroudingBombs.ToString(),
            CellTypeEnum.Bomb => "*",
            _ => string.Empty
        };

        private int GetCellNumberForUIRender()
        {
            if (!IsExposed && IsGuess)
                return 11;

            if (!IsExposed)
                return 0;

            return CellType switch
            {
                CellTypeEnum.Blank => 9,
                CellTypeEnum.Number => NumberOfSurroudingBombs,
                CellTypeEnum.Bomb => 10,
                _ => 0
            };
        }

        public static bool IsNeighboorInBounds(int boardSize, int neighboorRow, int neighboorColumn)
        {
            return neighboorRow >= 0 && neighboorRow < boardSize && neighboorColumn >= 0 && neighboorColumn < boardSize;
        }
    }
}