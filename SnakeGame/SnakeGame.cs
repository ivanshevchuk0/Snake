using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class SnakeGame
    {
        static int width = 20;
        static int height = 20;
        static char[,] field = new char[width, height];
        static List<(int x, int y)> snake = new List<(int x, int y)>();
        static (int x, int y) apple;
        static Random random = new Random();
        static ConsoleKey direction = ConsoleKey.RightArrow;

        static void Initialize()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    field[i, j] = ' ';
                }
            }
            snake.Add((width / 2, height / 2));
            GenerateApple();
        }

        static void GenerateApple()
        {
            do
            {
                apple = (random.Next(0, width), random.Next(0, height));
            } while (snake.Contains(apple));
            field[apple.x, apple.y] = '*';
        }

        static void Draw()
        {
            Console.Clear();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (snake.Contains((i, j)))
                    {
                        Console.Write("O");
                    }
                    else
                    {
                        Console.Write(field[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        static void Input()
        {
            if(Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if(key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow || key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                {
                    direction = key;
                }
            }
        }

        static void UpdateSnake()
        {
            var head = snake[0];
            (int x, int y) newHead = head;

            switch(direction)
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
            snake.Insert(0, newHead);
            if(newHead == apple)
            {
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
            if(head.x < 0 || head.y < 0 || head.x >= width || head.y >= height || snake.GetRange(1, snake.Count - 1).Contains(head))
            {
                Console.Clear();
                Console.WriteLine("Game Over!");
                System.Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
                
            }
        }

        static void Main(string[] args)
        {
            Initialize();
            while(true)
            {
                Draw();
                Input();
                UpdateSnake();
                CheckCollision();
                Thread.Sleep(100);
            }
        }
    }
}
