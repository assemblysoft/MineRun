using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using Xunit;

using MineRun.Shared.Models;

namespace MineRun.Game.Tests
{
    /// <summary>
    /// Confirm Passable or Unpassable routes
    /// </summary>
    public class RouteAnalyserTests
    {       

        [Fact]
        public void InvalidTest()
        {
            MineState[,] testState =
                {   { MineState.None, MineState.Mine, MineState.None, MineState.None},
                    { MineState.None, MineState.None, MineState.Mine, MineState.None}};
            RouteAnalyser check = new RouteAnalyser(testState, 2, 4);
            Assert.False(check.CheckValidRouteExists(0, 0));
        }
        [Fact]
        public void ValidTest()
        {
            MineState[,] testState =
                {   { MineState.None, MineState.Mine, MineState.None, MineState.None},
                    { MineState.None, MineState.None, MineState.None, MineState.Mine}};
            RouteAnalyser check = new RouteAnalyser(testState, 2, 4);
            Assert.True(check.CheckValidRouteExists(0, 0));
        }

    }

}
