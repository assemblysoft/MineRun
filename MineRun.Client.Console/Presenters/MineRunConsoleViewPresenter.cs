using System;
using System.Text.Json;

using MineRun.API;
using MineRun.Shared;
using MineRun.Shared.Models;
using MineRun.Game;


namespace MineRun.Client.Presenters
{
    public class MineRunConsoleViewPresenter : IViewPresenter, IGameDisplay
    {
        private GameBrain game;
        private GameState data;
        private string name;

        //ToDo: Move colors to injected options or config
        readonly ConsoleColor backColor = ConsoleColor.Black;
        readonly ConsoleColor mainColor = ConsoleColor.White;
        readonly ConsoleColor explodedColor = ConsoleColor.Gray;
        readonly ConsoleColor mineColor = ConsoleColor.Red;
        readonly ConsoleColor currentColor = ConsoleColor.Green;
        readonly ConsoleColor color1 = ConsoleColor.DarkYellow;
        readonly ConsoleColor color2 = ConsoleColor.Yellow;


        public void Start(string name = "Player1", Level level = Level.Level1)
        {
            this.name = name;

            switch (level)
            {
                case Level.Level1:
                    {
                        game = new LevelOne(this);
                        break;
                    }
                default:
                    {
                        game = new LevelOne(this);
                        break;
                    }
            }

            bool isRunning = true;
            while (isRunning)
            {             
                ConsoleKeyInfo input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        game.Move(Movement.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        game.Move(Movement.Down);
                        break;
                    case ConsoleKey.RightArrow:
                        game.Move(Movement.Right);
                        break;
                    case ConsoleKey.LeftArrow:
                        game.Move(Movement.Left);
                        break;
                    case ConsoleKey.S:
                        game.IsShow = !game.IsShow;
                        break;
                    case ConsoleKey.Enter:
                        game.Restart();
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q:
                        isRunning = false;
                        break;
                    default:
                        ConsoleRender();
                        break;
                }
            }
        }

        public void OnFailed(string gameState)
        {
            data = JsonSerializer.Deserialize<GameState>(gameState);
            ConsoleDrawFailed();
        }

        public void OnSuccess(string gameState)
        {
            data = JsonSerializer.Deserialize<GameState>(gameState);
            ConsoleDrawSuccess();
        }

        public void OnUpdate(string gameState)
        {
            data = JsonSerializer.Deserialize<GameState>(gameState);
            ConsoleRender();
        }

        void ConsoleRender()
        {
            Console.ForegroundColor = mainColor;
            Console.BackgroundColor = backColor;
            Console.Clear();

            //  top axis
            Console.Write("    ");
            for (int i = 0; i < data.Cols; i++)
            {
                Console.Write($"   {(char)(i + 'A')}  ");
            }
            Console.Write("\n\n");

            // grid paint
            for (int i = 0; i < data.Rows; i++)
            {
                // cell top padding paint
                Console.Write("    ");
                for (int j = 0; j < data.Cols; j++)
                {
                    SetBackColor(i, j);
                    Console.Write("      ");
                }

                // grid left axis paint
                Console.BackgroundColor = backColor;
                Console.Write($"\n {i + 1}  ");

                // grid body paint
                for (int j = 0; j < data.Cols; j++)
                {
                    SetBackColor(i, j);
                    Console.Write("      ");
                }

                // grid right axis paint
                Console.BackgroundColor = backColor;
                Console.Write($"  {i + 1}");
                Console.Write("\n    ");

                // cell bottom padding paint
                for (int j = 0; j < data.Cols; j++)
                {
                    SetBackColor(i, j);
                    Console.Write("      ");
                }
                Console.BackgroundColor = backColor;
                Console.Write('\n');
            }
            Console.Write("\n   ");
            // grid bottom axis paint
            for (int i = 0; i < data.Cols; i++)
            {
                Console.Write($"   {(char)(i + 'A')}  ");
            }
            Console.Write('\n');
            Console.WriteLine($"Score:{data.Score}\tLives:{data.Lives}\tMoves:{data.Moves}\tCurrent Position:{(char)(data.CurrentPos.Col + 'A')},{data.CurrentPos.Row + 1}\n");
            Console.Write($"Ok {name}, Press Arrow keys to navigate. Press S key to show Mines (Cheet Mode). Q Quit.");
        }

        void ConsoleDrawFailed()
        {
            Console.Clear();
            Console.WriteLine($"Score:{data.Score}\tLives:{data.Lives}\tMoves:{data.Moves}\n\n");
            Console.WriteLine("     _____  ___     ____   __     _____  ___     __");
            Console.WriteLine("    / ___/ / _ |   /  _/  / /    /  __/ / _ \\   / /");
            Console.WriteLine("   /__/   / __ |  _/ /   / /__  /  _/  / // /  /_/");
            Console.WriteLine("  /_/    /_/ |_| /___/  /____/ /____/ /____/  (_)");
            Console.WriteLine($"Score:{data.Score}\tLives:{data.Lives}\tMoves:{data.Moves}\n\n");
            Console.WriteLine($"\n\n Press Enter to play again {name}");
        }
        void ConsoleDrawSuccess()
        {
            Console.Clear();
            Console.WriteLine($"Score:{data.Score}\tLives:{data.Lives}\tMoves:{data.Moves}\n\n");
            Console.WriteLine("Success!");
            Console.WriteLine(" _      __   ____   _  __   _  __   _____  ___    __");
            Console.WriteLine("| | /| / /  /   /  / \\/ /  / \\/ /  / ___/ / _ \\  / /");
            Console.WriteLine("| |/ |/ /  _/ /_  /    /  /    /  /  _/  / , _/ /_/");
            Console.WriteLine("|__/|__/  /____/ /_/|_/  /_/|_/  /____/ /_/|_| (_)");
            Console.WriteLine($"Score:{data.Score}\tLives:{data.Lives}\tMoves:{data.Moves}\n\n");
            Console.WriteLine($"\n\n Press Enter to try again {name}");
        }

        #region helpers

        void SetBackColor(int i, int j)
        {
            if (data.CurrentPos.Equal(i, j))
            {
                Console.BackgroundColor = currentColor;
                return;
            }
            if (data.ExplodedPositions.Count > 0)
            {
                foreach (Pos pos in data.ExplodedPositions)
                    if (pos.Equal(i, j))
                    {
                        Console.BackgroundColor = explodedColor;
                        return;
                    }
            }
            if (data.MineList != null)
            {
                foreach (Pos pos in data.MineList)
                    if (pos.Equal(i, j))
                    {
                        Console.BackgroundColor = mineColor;
                        return;
                    }
            }
            Console.BackgroundColor = ((i + j) % 2 == 0) ? color1 : color2;

        }
        #endregion
    }
}
