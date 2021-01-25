using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using MineRun.API;
using MineRun.Shared.Models;

namespace MineRun.Game
{
    public abstract class GameBrain
    {
        readonly IGameDisplay _display;

        public GameBrain(IGameDisplay display)
        {
            _display = display;
        }

        protected MineState[,] mineState; //2 dimensional array of mine states

        protected bool isHard;

        protected int lives;
        protected int mineCount;


        protected bool isShow;
        protected GameState gameData;
        protected List<Pos> mineList;

        /// <summary>
        /// Initialises the game 
        /// </summary>
        /// <param name="_rows">Rows on the board</param>
        /// <param name="_cols">Columns on the board</param>
        /// <param name="_lives">Starting lives</param>
        /// <param name="_isHard">Easy or Hard</param>
        protected void Initialise(int _rows, int _cols, int _lives, bool _isHard)
        {
            gameData = new GameState //game state
            {
                Rows = _rows,
                Cols = _cols,
                Lives = _lives,
                Moves = 0
            };

            isHard = _isHard;
                        
            int _fieldcount = _rows * _cols;
            CalculateMineCount(_isHard, _fieldcount);

            mineState = new MineState[_rows, _cols]; //mine collection
            mineList = new List<Pos>();
            Random rand = new Random();
            gameData.CurrentPos = new Pos(rand.Next(0, _rows), 0); //starting position
            int _mineCount = 0;
            int _tempIndex, _row, _col;
            while (_mineCount <= mineCount)
            {
                _tempIndex = rand.Next(0, _fieldcount); //random generate index of mine
                _col = _tempIndex % gameData.Cols; //column index
                _row = (_tempIndex - _col) / gameData.Cols; //row index

                //if position is start position or already placed, next iteration
                if (gameData.CurrentPos.Equal(_row, _col) || mineState[_row, _col] == MineState.Mine) 
                    continue;

                mineState[_row, _col] = MineState.Mine;
                mineList.Add(new Pos(_row, _col));
                _mineCount++;
            }

            if (new RouteAnalyser(mineState, gameData.Rows, gameData.Cols).CheckValidRouteExists(gameData.CurrentPos.Row, gameData.CurrentPos.Col))
            {//...valid route exists
                Update();
            }
            else
            {//...unable to navigate to end column
                Restart();
            }
        }

        /// <summary>
        /// Calculate the number of mines based on level
        /// </summary>
        /// <param name="_isHard"></param>
        /// <param name="_fieldcount"></param>
        private void CalculateMineCount(bool _isHard, int _fieldcount)
        {
            mineCount = (_isHard) ? (int)(_fieldcount * 0.3f) : (int)(_fieldcount * 0.10f); //percentage of mines that should be calculated
        }

        public bool IsShow
        {
            get { return isShow; }
            set
            {
                isShow = value;
                if (isShow)
                    gameData.MineList = mineList;
                else
                    gameData.MineList = null;

                Update();
            }
        }

        public void Failed()
        {
            string dataString = JsonSerializer.Serialize(gameData);
            _display.OnFailed(dataString); //pass serialized json to display
        }
        public void Success()
        {
            string dataString = JsonSerializer.Serialize(gameData);
            _display.OnSuccess(dataString);
        }

        protected void Update()
        {
            string dataString = JsonSerializer.Serialize(gameData);
            _display.OnUpdate(dataString);
        }

        public abstract bool Move(Movement move);
        public abstract void Restart();        
        
    }

    public class LevelOne : GameBrain
    {

        public LevelOne(IGameDisplay display) : base(display)
        {
            lives = 3;
            Initialise(8, 8, lives, false);
        }
        public LevelOne(IGameDisplay display, int _rows, int _cols, int _lives, bool _isHard) : base(display)
        {

            lives = _lives;
            Initialise(_rows, _cols, _lives, _isHard);
        }

        public override bool Move(Movement movement)
        {
            bool isVaild = false;
            switch (movement)
            {
                case Movement.Up:
                    
                    //check if target is on the boundary
                    if (gameData.CurrentPos.Row > 0 && mineState[gameData.CurrentPos.Row - 1, gameData.CurrentPos.Col] != MineState.Exploded)
                    {
                        if (mineState[gameData.CurrentPos.Row - 1, gameData.CurrentPos.Col] == MineState.Mine) //target is a mine
                        {
                            mineState[gameData.CurrentPos.Row - 1, gameData.CurrentPos.Col] = MineState.Exploded; //set to exploded
                            gameData.ExplodedPositions.Add(new Pos(gameData.CurrentPos.Row - 1, gameData.CurrentPos.Col)); //add to obstacle list
                            gameData.Lives--;
                        }
                        else
                            gameData.MoveUp(); //move current position
                        
                        isVaild = true; //valid position
                    }
                    break;
                case Movement.Down:
                    if (gameData.CurrentPos.Row < gameData.Rows - 1 && mineState[gameData.CurrentPos.Row + 1, gameData.CurrentPos.Col] != MineState.Exploded)
                    {

                        if (mineState[gameData.CurrentPos.Row + 1, gameData.CurrentPos.Col] == MineState.Mine)
                        {
                            mineState[gameData.CurrentPos.Row + 1, gameData.CurrentPos.Col] = MineState.Exploded;
                            gameData.ExplodedPositions.Add(new Pos(gameData.CurrentPos.Row + 1, gameData.CurrentPos.Col));
                            gameData.Lives--;
                        }
                        else
                            gameData.MoveDown();
                        isVaild = true;
                    }
                    break;
                case Movement.Left:
                    if (gameData.CurrentPos.Col > 0 && mineState[gameData.CurrentPos.Row, gameData.CurrentPos.Col - 1] != MineState.Exploded)
                    {

                        if (mineState[gameData.CurrentPos.Row, gameData.CurrentPos.Col - 1] == MineState.Mine)
                        {
                            mineState[gameData.CurrentPos.Row, gameData.CurrentPos.Col - 1] = MineState.Exploded;
                            gameData.ExplodedPositions.Add(new Pos(gameData.CurrentPos.Row, gameData.CurrentPos.Col - 1));
                            gameData.Lives--;
                        }
                        else
                            gameData.MoveLeft();
                        isVaild = true;
                    }
                    break;
                case Movement.Right:
                    if (gameData.CurrentPos.Col < gameData.Cols - 1 && mineState[gameData.CurrentPos.Row, gameData.CurrentPos.Col + 1] != MineState.Exploded)
                    {
                        if (mineState[gameData.CurrentPos.Row, gameData.CurrentPos.Col + 1] == MineState.Mine)
                        {
                            mineState[gameData.CurrentPos.Row, gameData.CurrentPos.Col + 1] = MineState.Exploded;
                            gameData.ExplodedPositions.Add(new Pos(gameData.CurrentPos.Row, gameData.CurrentPos.Col + 1));
                            gameData.Lives--;
                        }
                        else
                            gameData.MoveRight();
                        isVaild = true;
                    }
                    break;
            }
            
            if (isVaild)
                gameData.Moves++; //move count increase
                        
            
            if (gameData.Lives < 0) //check failure
            {
                gameData.Lives = 0;
                Failed();
            }
            else if (gameData.CurrentPos.Col >= gameData.Cols - 1) //check success
                Success();
            else if (isVaild)
                Update();
            
            
            return isVaild;
        }

        public override void Restart()
        {
            Initialise(gameData.Rows, gameData.Cols, lives, isHard);
        }
    }

    /// <summary>
    /// State data for the game
    /// </summary>
    public class GameState
    {
        public int Score { get; set; }
        public int Lives { get; set; }
        public int Moves { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        internal void MoveUp()
        {
            CurrentPos = new Pos(CurrentPos.Row - 1, CurrentPos.Col);
        }
        internal void MoveDown()
        {
            CurrentPos = new Pos(CurrentPos.Row + 1, CurrentPos.Col);
        }
        internal void MoveRight()
        {
            CurrentPos = new Pos(CurrentPos.Row, CurrentPos.Col + 1);
        }
        internal void MoveLeft()
        {
            CurrentPos = new Pos(CurrentPos.Row, CurrentPos.Col - 1);
        }
        public Pos CurrentPos { get; set; }
        public List<Pos> ExplodedPositions { get; set; }
        public List<Pos> MineList { get; set; }
        public GameState()
        {
            ExplodedPositions = new List<Pos>();
        }
    }
}
