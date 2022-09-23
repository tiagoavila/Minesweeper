using Minesweeper.Core;
using Minesweeper.Core.Entities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Game
{
    public class Game
    {
        private readonly Board _board;
        private readonly Sprite _boardUI;
        private readonly Dictionary<Vector2f, Cell> _boxes;
        private readonly RenderTexture renderTexture = new(320, 320);
        private readonly int boardSize = 20;

        public bool LeftClick { get; set; }
        public bool RightClick { get; set; }

        Sprite gameOver;
        Sprite youWon;

        bool isWon;
        bool isLose;

        public Game()
        {
            Texture texture = new("Assets/tileset.png");

            _board = new Board(boardSize, 40, texture);
            _board.InitializeBoard();

            _boardUI = new Sprite(renderTexture.Texture)
            {
                Position = new Vector2f(640 / 2 - 320 / 2, 480 / 2 - 320 / 2)
            };
            
            _boxes = _board.CellsDictionary;

            gameOver = new Sprite(new Texture("Assets/game-over.png"));
            youWon = new Sprite(new Texture("Assets/you-won.png"));

            gameOver.Position = new Vector2f(0, 160 - 25);
            youWon.Position = new Vector2f(0, 160 - 25);
        }

        public void Draw(RenderTarget window)
        {
            renderTexture.Clear();

            foreach (var key in _boxes.Keys)
            {
                renderTexture.Draw(_boxes[key].uiBox);
            }

            if (isLose)
            {
                renderTexture.Draw(gameOver);
            }

            if (isWon)
            {
                renderTexture.Draw(youWon);
            }

            renderTexture.Display();

            window.Draw(_boardUI);
        }

        public void Update(Vector2i mouseCoords)
        {
            foreach (KeyValuePair<Vector2f, Cell> box in _boxes)
            {
                if (LeftClick && box.Value.rect.Contains(mouseCoords.X, mouseCoords.Y))
                {
                    LeftClick = false;

                    UserPlayResult userPlayResult = _board.Play(new UserPlay(box.Value.Row, box.Value.Column, false));
                    if (userPlayResult != null && userPlayResult.IsSuccessful)
                    {
                        if (userPlayResult.ResultingGameState == Core.Enums.GameStateEnum.Won)
                        {
                            isWon = true;
                        }
                        //box.Value.uiBox.TextureRect = box.Value.CreateTextureRect();
                    }
                    else
                    {
                        _board.ExposeAllCells();
                        isLose = true;
                    }                    
                }

                // set flag
                if (RightClick && box.Value.rect.Contains(mouseCoords.X, mouseCoords.Y))
                {
                    RightClick = false;

                    if (!box.Value.IsExposed)
                    {
                        _board.Play(new UserPlay(box.Value.Row, box.Value.Column, true));
                    }
                }
            }
        }
    }
}
