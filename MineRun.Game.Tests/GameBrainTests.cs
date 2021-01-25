using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using Xunit;

using MineRun.Game.Tests.Mocks;
using MineRun.Shared.Models;

namespace MineRun.Game.Tests
{    
    
    public class GameBrainTests
    {
        readonly InMemoryGameDisplay display;

        public GameBrainTests()
        {
            display = new InMemoryGameDisplay();
        }

        /// <summary>
        /// Board contruction tests
        /// </summary>
        /// <remarks>
        /// testData: rows => 4, cols => 5, lives => 3, dificult => easy
        /// </remarks>
        [Fact]        
        public void CanConstructValidGameboard()
        {
            LevelOneGameMock testEngine = new LevelOneGameMock(display, 4, 5, 3);
            int mineCount = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (testEngine.GameState[i, j] == MineState.Mine) mineCount++;
                }

            Assert.Equal(4, testEngine.GameData.Rows); //row set
            Assert.Equal(5, testEngine.GameData.Cols); //col set
            Assert.Equal(3, testEngine.GameData.Lives); //lives set

            //3 Tests
            //Rows
            //Cols
            //Lives

            //ToDo: Mines
            //Assert.Equal((int)(4 * 5 * 0.2f), mineCount); //mine generation
        }

        /// <summary>
        /// Movement tests
        /// </summary>
        [Fact]
        public void MoveTest()
        {
            MineState[,] testState =
                {   { MineState.None, MineState.None},
                    { MineState.None, MineState.None}};

            LevelOneGameMock testEngine = new LevelOneGameMock(display, 5, 5, 3, testState); //input init data
            testEngine.GameData.CurrentPos = new Pos(0, 0); //start pos
            
            //move down test
            testEngine.Move(Movement.Down);
            GameState data = JsonSerializer.Deserialize<GameState>(display.GameState);
            Assert.Equal(1, data.Moves);
            Assert.True(data.CurrentPos.Equal(1, 0));

            //move right test
            testEngine.Move(Movement.Right);
            data = JsonSerializer.Deserialize<GameState>(display.GameState);
            Assert.Equal(2, data.Moves);
            Assert.True(data.CurrentPos.Equal(1, 1));
            
            //move up test
            testEngine.Move(Movement.Up);
            data = JsonSerializer.Deserialize<GameState>(display.GameState);
            Assert.Equal(3, data.Moves);
            Assert.True(data.CurrentPos.Equal(0, 1));
            
            //move left test
            testEngine.Move(Movement.Left);
            data = JsonSerializer.Deserialize<GameState>(display.GameState);
            Assert.Equal(4, data.Moves);
            Assert.True(data.CurrentPos.Equal(0, 0));
            
            //invalid move test
            testEngine.Move(Movement.Left);
            data = JsonSerializer.Deserialize<GameState>(display.GameState);
            Assert.Equal(4, data.Moves);
            Assert.True(data.CurrentPos.Equal(0, 0));

            //10 Validated Movement Tests
        }

        [Fact]
        public void MoveToMineAndObstacle()
        {
            MineState[,] testState =
                {   { MineState.None, MineState.Exploded},
                    { MineState.None, MineState.None}};

            LevelOneGameMock testEngine = new LevelOneGameMock(display, 5, 5, 3, testState);
            testEngine.GameData.CurrentPos = new Pos(0, 0);
            
            //move into mine test
            testEngine.Move(Movement.Right);
            Assert.True(testEngine.GameData.CurrentPos.Equal(0, 0));
            Assert.Equal(3, testEngine.GameData.Lives);
            Assert.Equal(MineState.Exploded, testEngine.GameState[0, 1]);
            
            //move into obstacle test (exploded mine)
            testEngine.Move(Movement.Right);
            Assert.True(testEngine.GameData.CurrentPos.Equal(0, 0));
            Assert.Equal(3, testEngine.GameData.Lives);

            //5 Validated Exploded Tests
        }

        [Fact]
        public void CanSuccessfullyNavigateBoard()
        {
            //Arrange

            MineState[,] testState =
                {   { MineState.None, MineState.Mine, MineState.None, MineState.None, MineState.None},
                    { MineState.None, MineState.None, MineState.None, MineState.Mine, MineState.Mine},
                    { MineState.Mine, MineState.None, MineState.None, MineState.None, MineState.None},
                    { MineState.None, MineState.Mine, MineState.None, MineState.Mine, MineState.None},
                    { MineState.None, MineState.Mine, MineState.None, MineState.None, MineState.None}};

            LevelOneGameMock testEngine = new LevelOneGameMock(display, 5, 5, 3, testState);
            
            
            //Act
            testEngine.GameData.CurrentPos = new Pos(0, 0);
            Assert.False(display.IsSuccess); //pre-test success
            
            testEngine.Move(Movement.Down);         // moved    moves = 1
            testEngine.Move(Movement.Left);         // ignore   boundry outside
            testEngine.Move(Movement.Right);        // moved    moves = 2
            testEngine.Move(Movement.Right);        // ignore   obstacle
            testEngine.Move(Movement.Up);           // explode   moves = 3 lives = 2
            testEngine.Move(Movement.Right);        // moved    moves = 4
            testEngine.Move(Movement.Down);         // moved    moves = 5
            testEngine.Move(Movement.Right);        // moved    moves = 6
            testEngine.Move(Movement.Right);        // moved    moves = 7
            
            Assert.True(display.IsSuccess); //sucesss flag test
            
            GameState data = JsonSerializer.Deserialize<GameState>(display.GameState); //tests deserialize from json
            Assert.Equal(7, data.Moves); //move count test
            Assert.Equal(2, data.Lives); //lives count test

            //4 Tests
            //7 Moves
            //2 Lives Remain
        }

        [Fact]
        public void TotalTestForFailed()
        {
            MineState[,] testState =
                {   { MineState.None, MineState.Mine, MineState.None, MineState.None, MineState.None},
                    { MineState.None, MineState.None, MineState.None, MineState.Mine, MineState.Mine},
                    { MineState.Mine, MineState.None, MineState.Mine, MineState.None, MineState.None},
                    { MineState.None, MineState.Mine, MineState.None, MineState.Mine, MineState.None},
                    { MineState.None, MineState.Mine, MineState.None, MineState.None, MineState.None}};

            LevelOneGameMock testEngine = new LevelOneGameMock(display, 5, 5, 3, testState);

            testEngine.GameData.CurrentPos = new Pos(1, 0); //start pos
            Assert.False(display.IsFailed); //failed flag pre-test

            testEngine.Move(Movement.Down);         // explode   moves = 1 lives = 2
            testEngine.Move(Movement.Right);        // moved    moves = 2
            testEngine.Move(Movement.Up);           // explode   moves = 3 lives = 1
            testEngine.Move(Movement.Right);        // moved    moves = 4
            testEngine.Move(Movement.Right);        // explode   moves = 5 lives = 0
            testEngine.Move(Movement.Down);         // explode   moves = 6 died
            Assert.True(display.IsFailed);
            
            GameState data = JsonSerializer.Deserialize<GameState>(display.GameState);
            Assert.Equal(6, data.Moves); //moves test
            Assert.Equal(0, data.Lives); //lives test

            //4 Tests
            //6 Moves
            //0 Lives Remain
        }

    }

}
