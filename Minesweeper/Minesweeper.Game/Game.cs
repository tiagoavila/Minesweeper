using Minesweeper.Core;
using Minesweeper.Core.Entities;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Minesweeper.Game
{
    public class Game
    {
        private readonly Board _board;
        private readonly Sprite _boardUI;
        private readonly Sprite _gameOver;
        private readonly Sprite _youWon;
        private readonly RenderTexture renderTexture = new(GameConstants.BOARD_WIDTH, GameConstants.BOARD_HEIGHT);
        private readonly Music _losingMusicPlayer;
        private readonly Music _winningMusicPlayer;

        private bool isWon;
        private bool isLose;

        public bool LeftClick { get; set; }
        public bool RightClick { get; set; }

        public Game()
        {
            Texture texture = new("Assets/tileset.png");

            _board = new Board(GameConstants.BOARD_SIZE, GameConstants.NUMBER_OF_BOMBS, texture);
            _board.InitializeBoard();

            _boardUI = new Sprite(renderTexture.Texture)
            {
                Position = new Vector2f(GameConstants.WINDOW_WIDTH / 2 - GameConstants.BOARD_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2 - GameConstants.BOARD_HEIGHT / 2)
            };

            _gameOver = new Sprite(new Texture("Assets/game-over.png"))
            {
                Position = new Vector2f(0, 160 - 25)
            };

            _youWon = new Sprite(new Texture("Assets/you-won.png"))
            {
                Position = new Vector2f(0, 160 - 25)
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

            foreach (var key in _board.CellsDictionary.Keys)
            {
                renderTexture.Draw(_board.CellsDictionary[key].UIBox);
            }

            if (isLose)
            {
                renderTexture.Draw(_gameOver);
            }

            if (isWon)
            {
                renderTexture.Draw(_youWon);
            }

            renderTexture.Display();
            window.Draw(_boardUI);
        }

        public void Update(Vector2i mouseCoords)
        {
            foreach (KeyValuePair<Vector2f, Cell> box in _board.CellsDictionary)
            {
                if (LeftClick && box.Value.Rect.Contains(mouseCoords.X, mouseCoords.Y))
                {
                    LeftClick = false;

                    UserPlayResult userPlayResult = _board.Play(new UserPlay(box.Value.Row, box.Value.Column, false));
                    if (userPlayResult != null && userPlayResult.IsSuccessful)
                    {
                        if (userPlayResult.ResultingGameState == Core.Enums.GameStateEnum.Won)
                        {
                            isWon = true;
                            _winningMusicPlayer.Play();
                        }
                    }
                    else
                    {
                        _board.ExposeAllCells();
                        isLose = true;

                        _losingMusicPlayer.Play();
                    }

                    break;
                }

                // set flag
                if (RightClick && box.Value.Rect.Contains(mouseCoords.X, mouseCoords.Y))
                {
                    RightClick = false;

                    if (!box.Value.IsExposed)
                    {
                        _board.Play(new UserPlay(box.Value.Row, box.Value.Column, true));
                    }

                    break;
                }
            }
        }
    }
}
