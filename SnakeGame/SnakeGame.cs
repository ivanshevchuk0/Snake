using System;

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
                snake.Add((width / 2, height / 2));
                GenerateApple();
            }
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
                        Console.WriteLine("O");
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

        static void Main(string[] args)
        {
            
        }
    }
}
