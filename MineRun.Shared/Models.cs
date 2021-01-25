using System;
using System.Collections.Generic;

namespace MineRun.Shared.Models
{   

    public enum Level
    {
        Level1 = 1,
        Level2,
        Level3
    }

    public enum Movement
    {
        Up,
        Down,
        Right,
        Left
    }
    public enum MineState
    {
        None,
        Mine,
        Exploded
    }
    public struct Pos
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Pos(int _row, int _col)
        {
            Row = _row;
            Col = _col;
        }
        public bool Equal(int _row, int _col)
        {
            return _col == Col && _row == Row;
        }
    }

    #region Unused InProgress

    //public class Game
    //{
    //    Difficulty difficulty;
    //    Board board;
    //    public Game(int rows, int columns, Difficulty difficulty = Difficulty.Easy)
    //    {
    //        this.difficulty = difficulty;

    //        board = new Board
    //        {
    //            Dimensions = new Size { Height = rows, Width = columns }
    //        };

    //    }

    //    void SetupMines()
    //    {

    //    }

    //    public enum Difficulty
    //    {
    //        Easy = 1,
    //        Medium,
    //        Hard
    //    }
    //}

    //public class Board
    //{
    //    public Size Dimensions { get; set; }

    //    public bool IsSquare
    //    {
    //        get
    //        {
    //            if (Dimensions.Height < 1 || Dimensions.Width < 1)
    //                return false;

    //            return Dimensions.Width == Dimensions.Height;
    //        }
    //    }

    //}

    //public struct Size
    //{
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //}

    //class Cell
    //{
    //    public int Row { get; set; } = -1;
    //    public int Column { get; set; } = -1;
    //}



    //class MineCell : Cell
    //{
    //    public bool Live { get; set; } //contains an unexploded mine

    //    public bool IsVisited { get; set; }

    //}

    ////return list of cells
    //class MineBoard : Board
    //{
    //    List<MineCell> _cells;

    //    /// <summary>
    //    /// Our current representation of our cells on the board
    //    /// </summary>
    //    List<MineCell> Cells
    //    {
    //        get
    //        {
    //            return _cells;
    //        }
    //    }

    //}

        #endregion
}
