using Minesweeper.Core;
using Minesweeper.Core.Entities;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System.Data.Common;

namespace Minesweeper.Game
{
    public class Game
    {
        private readonly Board _boardEngine;
        private readonly Sprite _boardUI;
        private readonly Sprite _gameOverMessage;
        private readonly Sprite _youWonMessage;
        private readonly RenderTexture renderTexture = new(GameConstants.BOARD_WIDTH, GameConstants.BOARD_HEIGHT);
        private readonly Music _losingMusicPlayer;
        private readonly Music _winningMusicPlayer;
        readonly int _leftPadding = GameConstants.WINDOW_WIDTH / 2 - GameConstants.BOARD_WIDTH / 2; //Spacing from the left of the window
        readonly int _topPadding = GameConstants.WINDOW_HEIGHT / 2 - GameConstants.BOARD_HEIGHT / 2; //Spacing from the top of the window

        private bool isWon;
        private bool isLose;

        public bool LeftClick { get; set; }
        public bool RightClick { get; set; }

        public Game()
        {
            Texture texture = new("Assets/gaming_SpriteSheet.png");

            _boardEngine = new Board(GameConstants.BOARD_SIZE, GameConstants.NUMBER_OF_BOMBS, texture);
            _boardEngine.InitializeBoard();

            _boardUI = new Sprite(renderTexture.Texture)
            {
                Position = new Vector2f(_leftPadding, _topPadding)
            };

            _gameOverMessage = new Sprite(new Texture("Assets/game-over.jpg"))
            {
                Position = new Vector2f(120, 200)
            };

            _youWonMessage = new Sprite(new Texture("Assets/you-win.jpg"))
            {
                Position = new Vector2f(120, 140)
            };

            _losingMusicPlayer = new Music("Assets/losing-sound.wav")
            {
                Volume = 100
            };

            _winningMusicPlayer = new Music("Assets/winning-sound.wav")
            {
                Volume = 100
            };
        }

        public void Draw(RenderTarget window)
        {
            renderTexture.Clear();

            foreach (var key in _boardEngine.CellsDictionary.Keys)
            {
                renderTexture.Draw(_boardEngine.CellsDictionary[key].UIBox);
            }

            if (isLose)
            {
                renderTexture.Draw(_gameOverMessage);
            } 
            else if (isWon)
            {
                renderTexture.Draw(_youWonMessage);
            }

            renderTexture.Display();
            window.Draw(_boardUI);
        }

        public void Update(Vector2i mouseCoords)
        {
            int row = Math.DivRem(mouseCoords.Y - _topPadding, 32, out int _);
            int column = Math.DivRem(mouseCoords.X - _leftPadding, 32, out int _);

            if (_boardEngine.CellsDictionary.TryGetValue((row, column), out Cell cell))
            {
                if (LeftClick)
                {
                    HandleLeftCLick(cell);
                } 
                else if (RightClick)
                {
                   HandleRightCLick(cell);
                }
            }
        }

        private void HandleLeftCLick(Cell cell)
        {
            LeftClick = false;

            UserPlayResult userPlayResult = _boardEngine.Play(new UserPlay(cell.Row, cell.Column, false));
            if (userPlayResult != null && userPlayResult.IsSuccessful)
            {
                if (userPlayResult.ResultingGameState == Core.Enums.GameStateEnum.Won)
                {
                    isWon = true;
                    _boardEngine.ExposeAllCells();
                    _winningMusicPlayer.Play();
                }
            }
            else
            {
                _boardEngine.ExposeAllCells();
                isLose = true;

                _losingMusicPlayer.Play();
            }
        }

        private void HandleRightCLick(Cell cell)
        {
            RightClick = false;

            if (!cell.IsExposed)
            {
                _boardEngine.Play(new UserPlay(cell.Row, cell.Column, true));
            }
        }
    }
}
