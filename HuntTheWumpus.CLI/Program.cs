using System;
using System.Collections.Generic;

namespace HuntTheWumpus.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string room = "[ ]";

            const int rowNumber = 10;
            const int columnNumber = 10;

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
            player.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(player);

            Bat bat = new Bat();
            bat.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(bat);

            Bat bat2 = new Bat();
            bat2.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(bat2);

            Bat bat3 = new Bat();
            bat3.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(bat3);

            Wumpus wumpus = new Wumpus();
            wumpus.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(wumpus);

            Pit pit = new Pit();
            pit.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(pit);

            Pit pit2 = new Pit();
            pit2.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(pit2);

            while (player.IsAlife && wumpus.IsAlife)
            {
                Console.Clear();
                PrintMap(rowNumber, columnNumber, map, gameObjects);

                Console.WriteLine();

                int x1 = player.Coordinates.X > 0 ? player.Coordinates.X - 1 : 0; // Math.Max(0, player.Coordinates.X - 1);
                int x2 = player.Coordinates.X < (columnNumber - 1) ? player.Coordinates.X + 1 : columnNumber - 1; // Math.Min(columnNumber - 1, player.Coordinates.X + 1);
                int y1 = player.Coordinates.Y > 0 ? player.Coordinates.Y - 1 : 0; // Math.Max(0, player.Coordinates.Y - 1);
                int y2 = player.Coordinates.Y < (rowNumber - 1) ? player.Coordinates.Y + 1 : rowNumber - 1; // Math.Min(columnNumber - 1, player.Coordinates.Y + 1);

                bool isBatNear = false;
                bool isPitNear = false;
                bool isWumpusNear = false;

                for (int i = x1; i <= x2; i++)
                {
                    for (int j = y1; j <= y2; j++)
                    {
                        foreach (var gameobject in gameObjects)
                        {
                            if (gameobject is Player)
                            {
                                continue;
                            }

                            if (new Coordinates(i, j).CompareTo(gameobject.Coordinates))
                            {
                                if (gameobject is Bat)
                                {
                                    isBatNear = true;
                                }

                                if (gameobject is Pit)
                                {
                                    isPitNear = true;
                                }

                                if (gameobject is Wumpus)
                                {
                                    isWumpusNear = true;
                                }
                            }
                        }
                    }
                }

                if (isBatNear == true)
                {
                    Console.WriteLine(" - I hear rustling of wings");
                }

                if (isPitNear == true)
                {
                    Console.WriteLine(" - I hear wind blowing");
                }

                if (isWumpusNear == true)
                {
                    Console.WriteLine(" - I smell stink");
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(false);
                bool hasPlayerActed = false;

                if (keyInfo.Key == ConsoleKey.W && player.Coordinates.Y > 0)
                {
                    if (wumpus.Coordinates.Y == player.Coordinates.Y - 1 && wumpus.Coordinates.X == player.Coordinates.X)
                    {
                        wumpus.IsAlife = false;
                    }
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.S && player.Coordinates.Y < (rowNumber - 1))
                {
                    if (wumpus.Coordinates.Y == player.Coordinates.Y + 1 && wumpus.Coordinates.X == player.Coordinates.X)
                    {
                        wumpus.IsAlife = false;
                    }
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.D && player.Coordinates.X < (columnNumber - 1))
                {
                    if (wumpus.Coordinates.X == player.Coordinates.X + 1 && wumpus.Coordinates.Y == player.Coordinates.Y)
                    {
                        wumpus.IsAlife = false;
                    }
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.A && player.Coordinates.X > 0)
                {
                    if (wumpus.Coordinates.X == player.Coordinates.X - 1 && wumpus.Coordinates.Y == player.Coordinates.Y)
                    {
                        wumpus.IsAlife = false;
                    }
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.UpArrow && player.Coordinates.Y > 0)
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    player.Coordinates.Y--;
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.DownArrow && player.Coordinates.Y < (rowNumber - 1))
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    player.Coordinates.Y++;
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.LeftArrow && player.Coordinates.X > 0)
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    player.Coordinates.X--;
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.RightArrow && player.Coordinates.X < (columnNumber - 1))
                {
                    map[player.Coordinates.Y, player.Coordinates.X] = room;
                    player.Coordinates.X++;
                    hasPlayerActed = true;
                }

                if (keyInfo.Key == ConsoleKey.Z)
                {
                    player.IsAlife = false;
                }

                if (hasPlayerActed)
                {
                    if (wumpus.Coordinates.Y == player.Coordinates.Y && wumpus.Coordinates.X == player.Coordinates.X)
                    {
                        player.IsAlife = false;
                        Console.WriteLine("Wumpus caught you");
                        break;
                    }

                    map[wumpus.Coordinates.Y, wumpus.Coordinates.X] = room;
                    MoveWumpus(wumpus, rowNumber, columnNumber);

                    foreach (var gameObject in gameObjects)
                    {
                        if (gameObject is Player)
                        {
                            continue;
                        }

                        if (gameObject.Coordinates.Y == player.Coordinates.Y && gameObject.Coordinates.X == player.Coordinates.X)
                        {
                            if (gameObject is Pit)
                            {
                                player.IsAlife = false;
                                Console.WriteLine("You fell down!");
                            }

                            if (gameObject is Bat)
                            {
                                player.Coordinates.Y = random.Next(0, rowNumber);
                                player.Coordinates.X = random.Next(0, columnNumber);
                            }

                            if (gameObject is Wumpus)
                            {
                                player.IsAlife = false;
                                Console.WriteLine("Wumpus caught you");
                            }
                        }
                    }
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

        private static void MoveWumpus(Wumpus wumpus, int rowNumber, int columnNumber)
        {
            Random random = new Random();

            int possibleXmove = random.Next(-1, 2);
            int possibleYmove = 0;

            if (wumpus.Coordinates.X == 0)
            {
                possibleXmove = random.Next(0, 2);
            }
            else if (wumpus.Coordinates.X == (columnNumber - 1))
            {
                possibleXmove = random.Next(-1, 1);
            }

            if (possibleXmove == 0 && wumpus.Coordinates.Y == 0)
            {
                possibleYmove = random.Next(0, 2);
            }
            else if (possibleXmove == 0 && wumpus.Coordinates.Y == (rowNumber - 1))
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

        private static Coordinates Generator(List<GameObject> gameObjects, int rowNumber, int columnNumber)
        {
            Random random = new Random();
            Coordinates cords = new Coordinates(0, 0);
            bool isContinue = false;

            do
            {
                cords.X = random.Next(0, columnNumber);
                cords.Y = random.Next(0, rowNumber);

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
            //return "[X]";
            return "[ ]";
        }
    }

    public class Pit : GameObject
    {
        public override string Render()
        {
            //return "[O]";
            return "[ ]";
        }
    }

    public class Bat : GameObject
    {
        public override string Render()
        {
            return "[M]";
            //return "[ ]";
        }
    }
}