using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class SnakeGame
    {
        static int width = 40;
        static int height = 20;
        static char[,] field = new char[width, height];
        static List<(int x, int y)> snake = new List<(int x, int y)>();
        static (int x, int y) apple;
        static Random random = new Random();
        static ConsoleKey direction = ConsoleKey.RightArrow;
        static int score = 0;
        static int menuIndex = 0;
        static int restartIndex = 0;
        static string[] menuItems = { "Start", "Exit" };
        static string[] restartMenuItems = { "Restart", "Exit" };

        static void Initialize()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    field[i, j] = ' ';
                }
            }
            snake.Clear();
            snake.Add((width / 2, height / 2));
            GenerateApple();
        }

        static void GenerateApple()
        {
            do
            {
                apple = (random.Next(0, width), random.Next(0, height));
            } while (snake.Contains(apple));
        }

        static void Draw()
        {
            if (Console.IsOutputRedirected == false)
            {
                Console.Clear();
            }

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            if (consoleWidth < width + 2 || consoleHeight < height + 2)
            {
                Console.WriteLine("Console window too small. Please resize.");
                return;
            }

            int offsetX = (consoleWidth - width - 2) / 2;
            int offsetY = (consoleHeight - height - 2) / 2;

            for (int i = 0; i < offsetY; i++)
            {
                Console.WriteLine();
            }

            for (int i = 0; i < offsetX; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine(new string('#', width + 2));

            for (int y = 0; y < height; y++)
            {
                for (int i = 0; i < offsetX; i++)
                {
                    Console.Write(" ");
                }

                Console.Write("#");

                for (int x = 0; x < width; x++)
                {
                    if (snake.Contains((x, y)))
                    {
                        Console.Write("O");
                    }
                    else if ((x, y) == apple)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine("#");

            }
            for (int i = 0; i < offsetX; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine(new string('#', width + 2));

            //score
            Console.SetCursorPosition(offsetX + offsetX / 2, offsetY - 2);
            Console.WriteLine($"Score: {score}");
        }

        static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && direction != ConsoleKey.DownArrow ||
                    key == ConsoleKey.DownArrow && direction != ConsoleKey.UpArrow ||
                    key == ConsoleKey.LeftArrow && direction != ConsoleKey.RightArrow ||
                    key == ConsoleKey.RightArrow && direction != ConsoleKey.LeftArrow)
                {
                    direction = key;
                }
            }
        }

        static void UpdateSnake()
        {
            var head = snake[0];
            (int x, int y) newHead = head;

            switch (direction)
            {
                case ConsoleKey.UpArrow:
                    newHead = (head.x, head.y - 1);
                    break;
                case ConsoleKey.DownArrow:
                    newHead = (head.x, head.y + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    newHead = (head.x - 1, head.y);
                    break;
                case ConsoleKey.RightArrow:
                    newHead = (head.x + 1, head.y);
                    break;
            }

            if (newHead.x < 0) newHead.x = width - 1;
            if (newHead.y < 0) newHead.y = height - 1;
            if (newHead.x >= width) newHead.x = 0;
            if (newHead.y >= height) newHead.y = 0;

            snake.Insert(0, newHead);
            if (newHead == apple)
            {
                score++;
                GenerateApple();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void CheckCollision()
        {
            var head = snake[0];
            if (snake.GetRange(1, snake.Count - 1).Contains(head))
            {
                GameOver();
            }
        }

        static void GameOver()
        {
            Console.Clear();
            Console.WriteLine("Game Over!");
            Console.WriteLine($"Score: {score}");
            bool needRedraw = true;
            while (true)
            {
                if (needRedraw)
                {
                    for (int i = 0; i < restartMenuItems.Length; i++)
                    {
                        if (i == restartIndex)
                        {
                            Console.WriteLine($"> {restartMenuItems[i]}");
                        }
                        else
                        {
                            Console.WriteLine($" {restartMenuItems[i]}");
                        }
                    }
                    needRedraw = false;
                }
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            restartIndex = (restartIndex - 1 + restartMenuItems.Length) % restartMenuItems.Length;
                            needRedraw = true;
                            break;
                        case ConsoleKey.DownArrow:
                            restartIndex = (restartIndex + 1) % restartMenuItems.Length;
                            needRedraw = true;
                            break;
                        case ConsoleKey.Enter:
                            if (restartMenuItems[restartIndex] == "Restart")
                            {
                                StartMenu();
                                return;
                            }
                            else if (restartMenuItems[restartIndex] == "Exit")
                            {
                                Environment.Exit(0);
                            }
                            break;
                    }
                    if (needRedraw)
                    {
                        Console.Clear();
                        Console.WriteLine("Game Over!");
                        Console.WriteLine($"Score: {score}");
                    }
                }
            }
        }

        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Main Menu: ");
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == menuIndex)
                {
                    Console.WriteLine($"> {menuItems[i]}");
                }
                else
                {
                    Console.WriteLine($" {menuItems[i]}");
                }
            }

        }

        static void MainMenuInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        menuIndex = (menuIndex - 1 + menuItems.Length) % menuItems.Length;
                        MainMenu();
                        break;
                    case ConsoleKey.DownArrow:
                        menuIndex = (menuIndex + 1) % menuItems.Length;
                        MainMenu();
                        break;
                    case ConsoleKey.Enter:
                        if (menuItems[menuIndex] == "Start")
                        {
                            StartMenu();
                        }
                        else if (menuItems[menuIndex] == "Exit")
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
            }
        }

        static void StartMenu()
        {
            Initialize();
            while (true)
            {
                Draw();
                Input();
                UpdateSnake();
                CheckCollision();
                Thread.Sleep(100);
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                MainMenu();
                MainMenuInput();
                Thread.Sleep(100);
            }
        }
    }
}