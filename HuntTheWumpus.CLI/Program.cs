using System;

namespace HuntTheWumpus.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string room = "[ ]";

            const int rowNumber = 6;
            const int columnNumber = 6;

            string[,] map = new string[rowNumber, columnNumber];

            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < columnNumber; j++)
                {
                    map[i, j] = room;
                }
            }

            Random random = new Random();

            string player = "[@]";
            int x = random.Next(0, 5);
            int y = random.Next(0, 5);

            map[y, x] = player;

            string wumpus = "[X]";
            int a = random.Next(0, 5);
            int b = random.Next(0, 5);

            map[a, b] = wumpus;

            string pit = "[O]";
            int a1 = random.Next(0, 5);
            int b1 = random.Next(0, 5);

            map[a1, b1] = pit;

            string pit2 = "[O]";
            int a2 = random.Next(0, 5);
            int b2 = random.Next(0, 5);

            map[a2, b2] = pit2;

            string bat = "[M]";
            int a3 = random.Next(0, 5);
            int b3 = random.Next(0, 5);

            map[a3, b3] = bat;

            string bat2 = "[M]";
            int a4 = random.Next(0, 5);
            int b4 = random.Next(0, 5);

            map[a4, b4] = bat2;


            bool isPlayerAlife = true;

            while (isPlayerAlife)
            {
                Console.Clear();
                PrintMap(rowNumber, columnNumber, map);

                ConsoleKeyInfo keyInfo = Console.ReadKey(false);

                if (keyInfo.Key == ConsoleKey.UpArrow && y > 0)
                {
                    map[y, x] = room;
                    map[y - 1, x] = player;
                    y--;
                }

                if (keyInfo.Key == ConsoleKey.D)
                {
                    isPlayerAlife = false;
                }
            }

            Console.ReadKey();
        }

        private static void PrintMap(int rowNumber, int columnNumber, string[,] map)
        {
            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < columnNumber; j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}