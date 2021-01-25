using System;
using System.Collections.Generic;
using System.Text;

using MineRun.Shared.Models;

namespace MineRun.Game
{
    /// <summary>
    /// Provides insights on the navigation routes
    /// </summary>
    public class RouteAnalyser
    {
        readonly bool[,] routeCheckArray;
        private readonly int Rows;
        private readonly int Cols;

        public RouteAnalyser(MineState[,] mineStateArray, int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            routeCheckArray = new bool[rows, cols]; //two dimensions to represent the board
            
            CopyMineLocations(mineStateArray, rows, cols); //take snapshot of mine locations
        }

        
        private void CopyMineLocations(MineState[,] _stateArray, int rows, int cols)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    if (_stateArray[i, j] == MineState.Mine)
                        routeCheckArray[i, j] = true;
                }
        }

        /// <summary>
        /// Determines whether a valid route exists from current position
        /// </summary>
        /// <param name="_row"></param>
        /// <param name="_col"></param>
        /// <returns>valid route exists</returns>
        /// <remarks>
        /// Mines are created randomly. This sometimes leads to unpassable routes. 
        /// This method enables a re-create of the board until a valid route is confirmed.
        /// </remarks>
        public bool CheckValidRouteExists(int _row, int _col)
        {
            if (_col == Cols - 1) //reached end column, valid route exists
                return true;
            
            routeCheckArray[_row, _col] = true; //current pos

            if (_row < Rows - 1 && !routeCheckArray[_row + 1, _col]) //check up
                if (CheckValidRouteExists(_row + 1, _col))
                    return true;
            
            if (_row > 0 && !routeCheckArray[_row - 1, _col]) //check down
                if (CheckValidRouteExists(_row - 1, _col))
                    return true;

            if (_col < Cols - 1 && !routeCheckArray[_row, _col + 1]) //check right
                if (CheckValidRouteExists(_row, _col + 1))
                    return true;

            if (_col > 0 && !routeCheckArray[_row, _col - 1]) //check left
                if (CheckValidRouteExists(_row, _col - 1))
                    return true;

            return false; //blocked, not possible to navigate on this route
        }
    }
}
