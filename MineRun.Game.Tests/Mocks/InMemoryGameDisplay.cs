using System;
using System.Collections.Generic;
using System.Text;

using MineRun.API;

namespace MineRun.Game.Tests
{
    class InMemoryGameDisplay : IGameDisplay
    {        
        public string GameState { get; private set; }
        public bool IsFailed { get; private set; }
        public bool IsSuccess { get; private set; }               

        public void OnFailed(string gameState)
        {
            IsFailed = true;
            GameState = gameState;
        }

        public void OnSuccess(string gameState)
        {
            IsSuccess = true;
            GameState = gameState;
        }

        public void OnUpdate(string gameState)
        {
            GameState = gameState;
        }
    }
}
