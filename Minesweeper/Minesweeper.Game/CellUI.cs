using Minesweeper.Core;
using Minesweeper.Core.Enums;
using SFML.Graphics;
using SFML.System;

namespace Minesweeper.Game
{
    public class CellUI
    {
        public CellUI(int row, int column, Texture texture)
        {
            Cell = new Cell(row, column);
            Texture = texture;
            Position = new Vector2f(column * GameConstants.CELL_SIZE, row * GameConstants.CELL_SIZE);
            UIBox = new RectangleShape();
        }

        public CellUI(Cell cell, Texture texture)
        {
            Cell = cell;
            Texture = texture;
            Position = new Vector2f(cell.Column * GameConstants.CELL_SIZE, cell.Row * GameConstants.CELL_SIZE);
            UIBox = new RectangleShape();
        }

        public Cell Cell { get; private set; }
        public RectangleShape UIBox { get; private set; }
        public Texture Texture { get; private set; }
        public IntRect Rect { get; private set; }
        protected Vector2f Position { get; private set; }

        /// <summary>
        /// Creates the necessary UI objects to render a Cell
        /// </summary>
        public void CreateUiBox()
        {
            UIBox = new RectangleShape(new Vector2f(GameConstants.CELL_SIZE, GameConstants.CELL_SIZE))
            {
                Texture = Texture,
                Position = Position,
                TextureRect = CreateTextureRect()
            };

            int leftPadding = GameConstants.WINDOW_WIDTH / 2 - GameConstants.BOARD_WIDTH / 2;
            int topPadding = GameConstants.WINDOW_HEIGHT / 2 - GameConstants.BOARD_HEIGHT / 2;

            Rect = new IntRect(leftPadding + (int)Position.X, topPadding + (int)Position.Y, GameConstants.CELL_SIZE, GameConstants.CELL_SIZE);
        }

        /// <summary>
        /// Renders the Cell again after any event
        /// </summary>
        public void ReRenderCell()
        {
            UIBox.TextureRect = CreateTextureRect();
        }

        private IntRect CreateTextureRect()
        {
            (int left, int top) = GetLeftAndTopForUIRender();
            return new IntRect(left, top, GameConstants.RECTANGLE_SIZE, GameConstants.RECTANGLE_SIZE);
        }

        /// <summary>
        /// Get the pixels from the top and left of the Asset image gaming_SpriteSheet.png to render the correct image for the type of cell
        /// </summary>
        /// <returns></returns>
        private (int left, int top) GetLeftAndTopForUIRender()
        {
            if (!Cell.IsExposed && Cell.IsGuess)
                return (96, 64);

            if (!Cell.IsExposed)
                return (32, 64);

            int top = Math.DivRem(Cell.NumberOfSurroudingBombs, 4, out int left);

            return Cell.CellType switch
            {
                CellTypeEnum.Blank => (0, 0),
                CellTypeEnum.Number => (left * 32, top * 32),
                CellTypeEnum.Bomb => (64, 64),
                _ => (32, 64)
            };
        }
    }
}
