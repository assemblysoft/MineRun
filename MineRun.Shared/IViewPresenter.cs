using System;

using MineRun.Shared.Models;

namespace MineRun.Shared
{
    public interface IViewPresenter
    {
        void Start(string name, Level level);
    }

}
