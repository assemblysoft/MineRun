using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Xunit;

//namespace MineRun.Game.Tests
//{
//    public class BoardTests
//    {
//        [Theory]
//        [ClassData(typeof(BoardTestData))]
//        public void CanAddTheoryClassData(int value1, int value2, int expected)
//        {
//            //var board = new Board();

//            //var result = calculator.Add(value1, value2);

//            //Assert.Equal(expected, result);
//        }

//        [Theory]
//        [InlineData(8,8)]
//        [InlineData(10, 10)]
//        [InlineData(20, 20)]
//        public void BoardIsSquare(int height, int width)
//        {
//            //Arrange
//            var board = new Board
//            {
//                Dimensions = new Size { Height = height, Width = width }
//            };

//            //Act
//            var isSquare = board.IsSquare;

//            //Assert
//            Assert.True(isSquare);
//        }

//        [Theory]
//        [InlineData(4, 8)]
//        [InlineData(7, 10)]
//        [InlineData(20, 21)]
//        public void NonSquareBoardIsInvalid(int height, int width)
//        {
//            //Arrange
//            var board = new Board();
//            board.Dimensions = new Size { Height = height, Width = width };

//            //Act
//            var isSquare = board.IsSquare;

//            //Assert
//            Assert.False(isSquare);
//        }


//    }


//    public class BoardTestData : IEnumerable<object[]>
//    {
//        public IEnumerator<object[]> GetEnumerator()
//        {
//            yield return new object[] { 1, 2, 3 };
//            yield return new object[] { -4, -6, -10 };
//            yield return new object[] { -2, 2, 0 };
//            yield return new object[] { int.MinValue, -1, int.MaxValue };
//        }

//        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//    }
//}

