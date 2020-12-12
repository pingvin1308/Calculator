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

            Coordinates playersCoordinates = new Coordinates(random.Next(0, 5), random.Next(0, 5));
            //(int, int)(x, y) = Method();

            string wumpus = "[X]";

            Coordinates wumpusCoordinates = new Coordinates(0, 0);
            do
            {
                wumpusCoordinates.X = random.Next(0, 5);
                wumpusCoordinates.Y = random.Next(0, 5);
            } while (wumpusCoordinates.CompareTo(playersCoordinates));

            //(int, int)(x1, y1) = Method(x, y);

            string pit = "[O]";
            Coordinates pitCoordinates = new Coordinates(0, 0);
            do
            {
                pitCoordinates.X = random.Next(0, 5);
                pitCoordinates.Y = random.Next(0, 5);
            } while (pitCoordinates.CompareTo(wumpusCoordinates) || pitCoordinates.CompareTo(playersCoordinates));

            //(int, int)(x2, y2) = Method(x, y, x1, y1);

            string pit2 = "[O]";
            Coordinates pit2Coordinates = new Coordinates(0, 0);
            do
            {
                pit2Coordinates.X = random.Next(0, 5);
                pit2Coordinates.Y = random.Next(0, 5);
            } while (pit2Coordinates.CompareTo(wumpusCoordinates) || pit2Coordinates.CompareTo(playersCoordinates) || pit2Coordinates.CompareTo(pitCoordinates));

            string bat = "[M]";
            Coordinates batCoordinates = new Coordinates(0, 0);
            do
            {
                batCoordinates.X = random.Next(0, 5);
                batCoordinates.Y = random.Next(0, 5);
            } while (batCoordinates.CompareTo(wumpusCoordinates) || batCoordinates.CompareTo(playersCoordinates) || batCoordinates.CompareTo(pitCoordinates) || batCoordinates.CompareTo(pit2Coordinates));

            string bat2 = "[M]";
            Coordinates bat2Coordinates = new Coordinates(0, 0);
            do
            {
                bat2Coordinates.X = random.Next(0, 5);
                bat2Coordinates.Y = random.Next(0, 5);
            } while (bat2Coordinates.CompareTo(wumpusCoordinates) || bat2Coordinates.CompareTo(playersCoordinates) || bat2Coordinates.CompareTo(pitCoordinates) || bat2Coordinates.CompareTo(pit2Coordinates) || bat2Coordinates.CompareTo(batCoordinates));


            map[playersCoordinates.Y, playersCoordinates.X] = player;
            map[wumpusCoordinates.Y, wumpusCoordinates.X] = wumpus;
            map[pitCoordinates.Y, pitCoordinates.X] = pit;
            map[pit2Coordinates.Y, pit2Coordinates.X] = pit2;
            map[batCoordinates.Y, batCoordinates.X] = bat;
            map[bat2Coordinates.Y, bat2Coordinates.X] = bat2;

            bool isPlayerAlife = true;

            while (isPlayerAlife)
            {
                Console.Clear();
                PrintMap(rowNumber, columnNumber, map);

                ConsoleKeyInfo keyInfo = Console.ReadKey(false);

                if (keyInfo.Key == ConsoleKey.UpArrow && playersCoordinates.Y > 0)
                {
                    map[playersCoordinates.Y, playersCoordinates.X] = room;
                    map[playersCoordinates.Y - 1, playersCoordinates.X] = player;
                    playersCoordinates.Y--;
                }

                if (keyInfo.Key == ConsoleKey.DownArrow && playersCoordinates.Y < (rowNumber - 1))
                {
                    map[playersCoordinates.Y, playersCoordinates.X] = room;
                    map[playersCoordinates.Y + 1, playersCoordinates.X] = player;
                    playersCoordinates.Y++;
                }

                if (keyInfo.Key == ConsoleKey.LeftArrow && playersCoordinates.X > 0)
                {
                    map[playersCoordinates.Y, playersCoordinates.X] = room;
                    map[playersCoordinates.Y, playersCoordinates.X - 1] = player;
                    playersCoordinates.X--;
                }

                if (keyInfo.Key == ConsoleKey.RightArrow && playersCoordinates.X < (columnNumber - 1))
                {
                    map[playersCoordinates.Y, playersCoordinates.X] = room;
                    map[playersCoordinates.Y, playersCoordinates.X + 1] = player;
                    playersCoordinates.X++;
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

    public class Coordinates
    {
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public bool CompareTo(Coordinates b)
        {
            return X == b.X && Y == b.Y;
        }
    }
}