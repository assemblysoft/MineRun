using System;
using System.Collections.Generic;
using System.Text;

namespace MineRun.API
{

    public interface IGameDisplay
    {
        void OnUpdate(string gameState);
        void OnSuccess(string gameState);
        void OnFailed(string gameState);
    }    
}
