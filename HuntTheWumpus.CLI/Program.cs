using System;
using System.Collections.Generic;

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

            List<GameObject> gameObjects = new List<GameObject>();

            Player player = new Player();
            player.Coordinates = Generator(gameObjects);
            gameObjects.Add(player);

            Wumpus wumpus = new Wumpus();
            wumpus.Coordinates = Generator(gameObjects);
            gameObjects.Add(wumpus);

            Pit pit = new Pit();
            pit.Coordinates = Generator(gameObjects);
            gameObjects.Add(pit);

            Pit pit2 = new Pit();
            pit2.Coordinates = Generator(gameObjects);
            gameObjects.Add(pit2);

            Bat bat = new Bat();
            bat.Coordinates = Generator(gameObjects);
            gameObjects.Add(bat);

            Bat bat2 = new Bat();
            bat2.Coordinates = Generator(gameObjects);
            gameObjects.Add(bat2);

            while (player.IsAlife && wumpus.IsAlife)
            {
                Console.Clear();
                PrintMap(rowNumber, columnNumber, map, gameObjects);

                ConsoleKeyInfo keyInfo = Console.ReadKey(false);

                if (keyInfo.Key == ConsoleKey.W && player.Coordinates.Y > 0)
                {
                    if (wumpus.Coordinates.Y == player.Coordinates.Y - 1 && wumpus.Coordinates.X == player.Coordinates.X)
                    {
                        wumpus.IsAlife = false;
                    }
                }

                if (keyInfo.Key == ConsoleKey.S && player.Coordinates.Y < (rowNumber - 1))
                {
                    if (wumpus.Coordinates.Y == player.Coordinates.Y + 1 && wumpus.Coordinates.X == player.Coordinates.X)
                    {
                        wumpus.IsAlife = false;
                    }
                }

                if (keyInfo.Key == ConsoleKey.D && player.Coordinates.X < (columnNumber - 1))
                {
                    if (wumpus.Coordinates.X == player.Coordinates.X + 1 && wumpus.Coordinates.Y == player.Coordinates.Y)
                    {
                        wumpus.IsAlife = false;
                    }
                }

                if (keyInfo.Key == ConsoleKey.A && player.Coordinates.X > 0)
                {
                    if (wumpus.Coordinates.X == player.Coordinates.X - 1 && wumpus.Coordinates.Y == player.Coordinates.Y)
                    {
                        wumpus.IsAlife = false;
                    }
                }

                if (keyInfo.Key == ConsoleKey.UpArrow && player.Coordinates.Y > 0)
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    map[player.Coordinates.Y - 1, player.Coordinates.X] = player.Render();
                    player.Coordinates.Y--;
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = room;
                    MoveWumpus(wumpus);
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = wumpus.Render();
                }

                if (keyInfo.Key == ConsoleKey.DownArrow && player.Coordinates.Y < (rowNumber - 1))
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    map[player.Coordinates.Y + 1, player.Coordinates.X] = player.Render();
                    player.Coordinates.Y++;
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = room;
                    MoveWumpus(wumpus);
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = wumpus.Render();
                }

                if (keyInfo.Key == ConsoleKey.LeftArrow && player.Coordinates.X > 0)
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    map[player.Coordinates.Y, player.Coordinates.X - 1] = player.Render();
                    player.Coordinates.X--;
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = room;
                    MoveWumpus(wumpus);
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = wumpus.Render();
                }

                if (keyInfo.Key == ConsoleKey.RightArrow && player.Coordinates.X < (columnNumber - 1))
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    map[player.Coordinates.Y, player.Coordinates.X + 1] = player.Render();
                    player.Coordinates.X++;
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = room;
                    MoveWumpus(wumpus);
                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = wumpus.Render();
                }

                if (keyInfo.Key == ConsoleKey.Z)
                {
                    player.IsAlife = false;
                }
            }

            if (player.IsAlife == false)
            {
                Console.WriteLine("You Died!");
            }

            if (wumpus.IsAlife == false)
            {
                Console.WriteLine("You Won!");
            }

            Console.ReadKey();
        }

        private static void PrintMap(int rowNumber, int columnNumber, string[,] map, List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                map[gameObject.Coordinates.Y, gameObject.Coordinates.X] = gameObject.Render();
            }

            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < columnNumber; j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }

        private static void MoveWumpus(Wumpus wumpus)
        {
            Random random = new Random();

            int possibleXmove = random.Next(-1, 2);
            int possibleYmove = 0;

            if (wumpus.Coordinates.X == 0)
            {
                possibleXmove = random.Next(0, 2);
            }
            else if (wumpus.Coordinates.X == 5)
            {
                possibleXmove = random.Next(-1, 1);
            }

            if (possibleXmove == 0 && wumpus.Coordinates.Y == 0)
            {
                possibleYmove = random.Next(0, 2);
            }
            else if (possibleXmove == 0 && wumpus.Coordinates.Y == 5)
            {
                possibleYmove = random.Next(-1, 1);
            }
            else if (possibleXmove == 0)
            {
                possibleYmove = random.Next(-1, 2);
            }

            wumpus.Coordinates.X += possibleXmove;
            wumpus.Coordinates.Y += possibleYmove;
        }

        private static Coordinates Generator(List<GameObject> gameObjects)
        {
            Random random = new Random();
            Coordinates cords = new Coordinates(0, 0);
            bool isContinue = false;

            do
            {
                cords.X = random.Next(0, 5);
                cords.Y = random.Next(0, 5);

                foreach (GameObject gameObject in gameObjects)
                {
                    isContinue = false;

                    if (gameObject.Coordinates.CompareTo(cords))
                    {
                        isContinue = true;
                        break;
                    }
                }

            } while (isContinue);

            return cords;
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

    public class GameObject
    {
        public Coordinates Coordinates { get; set; }

        public virtual string Render()
        {
            return " ";
        }
    }

    public class Player : GameObject
    {
        public Player()
        {
            IsAlife = true;
        }

        public bool IsAlife { get; set; }

        public override string Render()
        {
            return "[@]";
        }
    }

    public class Wumpus : GameObject
    {
        public Wumpus()
        {
            IsAlife = true;
        }

        public bool IsAlife { get; set; }

        public override string Render()
        {
            return "[X]";
        }
    }

    public class Pit : GameObject
    {
        public override string Render()
        {
            return "[O]";
        }
    }

    public class Bat : GameObject
    {
        public override string Render()
        {
            return "[M]";
        }
    }

}