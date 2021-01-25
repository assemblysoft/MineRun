using System;
using System.Collections.Generic;
using System.Text;

using MineRun.API;
using MineRun.Shared.Models;

namespace MineRun.Game.Tests.Mocks
{
    class LevelOneGameMock : LevelOne
    {
        public LevelOneGameMock(IGameDisplay _display, int _rows, int _cols, int _lives, MineState[,] _gameState = null)
            : base(_display, _rows, _cols, _lives, false)
        {
            if (_gameState != null)
                mineState = _gameState;
        }

        public MineState[,] GameState
        {
            get { return mineState; }
            set { mineState = value; }
        }
        public GameState GameData
        {
            get { return gameData; }
            set { gameData = GameData; }
        }
    }
}
