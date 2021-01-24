using System;
using System.Collections.Generic;
using System.Linq;

namespace HuntTheWumpus.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to HuntTheWumpus");
            Console.WriteLine();
            Console.WriteLine("Choose difficulty: \"e\" for easy, \"n\" for normal,\"h\" for hard");
            ConsoleKeyInfo difficulty = Console.ReadKey(false);

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

            List<GameObject> gameObjects = InitializationGameObjects(rowNumber, columnNumber, difficulty);
            Player player = (Player)gameObjects.First(x => x is Player);
            Wumpus wumpus = (Wumpus)gameObjects.First(x => x is Wumpus);

            while (player.IsAlife && wumpus.IsAlife)
            {
                Console.Clear();
                PrintMap(rowNumber, columnNumber, map, gameObjects);

                Console.WriteLine();

                player.Weapon.ShowEquipement();

                CheckNearPlayer(rowNumber, columnNumber, player, gameObjects);

                bool hasPlayerActed = ActPlayer(wumpus, player, map, rowNumber, columnNumber, room);

                if (hasPlayerActed)
                {
                    GameProcess(wumpus, player, map, rowNumber, columnNumber, room, random, gameObjects);
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

        private static List<GameObject> InitializationGameObjects(int rowNumber, int columnNumber, ConsoleKeyInfo difficulty)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            Player player = new Player();
            player.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(player);

            int batCount = 0;
            int pitCount = 0;

            if (difficulty.Key == ConsoleKey.E)
            {
               batCount = 5;
               pitCount = 5;
            }

            if (difficulty.Key == ConsoleKey.N)
            {
                batCount = 10;
                pitCount = 10;
            }

            if (difficulty.Key == ConsoleKey.H)
            {
                batCount = 15;
                pitCount = 15;
            }

            for (int i = batCount; i > 0; i--)
            {
                Bat bat = new Bat();
                bat.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
                gameObjects.Add(bat);
            }

            for (int i = pitCount; i > 0; i--)
            {
                Pit pit = new Pit();
                pit.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
                gameObjects.Add(pit);
            }

            Wumpus wumpus = new Wumpus();
            wumpus.Coordinates = Generator(gameObjects, rowNumber, columnNumber);
            gameObjects.Add(wumpus);

            return gameObjects;
        }

        private static void CheckNearPlayer(int rowNumber, int columnNumber, Player player, List<GameObject> gameObjects)
        {
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
        }

        private static void GameProcess(Wumpus wumpus, Player player, string[,] map, int rowNumber, int columnNumber, string room, Random random, List<GameObject> gameObjects)
        {
            if (wumpus.Coordinates.Y == player.Coordinates.Y && wumpus.Coordinates.X == player.Coordinates.X)
            {
                player.IsAlife = false;
                Console.WriteLine("Wumpus caught you");
                return;
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

        private static bool ActPlayer(Wumpus wumpus, Player player, string[,] map, int rowNumber, int columnNumber, string room)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(false);

            if (keyInfo.Key == ConsoleKey.Q)
            {
                player.Weapon = new BowWeapon();
                return true;
            }

            if (keyInfo.Key == ConsoleKey.R)
            {
                player.Weapon = new MorningStarWeapon();
                return true;
            }

            if (keyInfo.Key == ConsoleKey.T)
            {
                player.Weapon = new SwordWeapon();
                return true;
            }
            if (player.Weapon.Attack(player, wumpus, rowNumber, columnNumber, keyInfo) == true)
            {
                return true;
            }

            if (keyInfo.Key == ConsoleKey.UpArrow && player.Coordinates.Y > 0)
            {
                map[player.Coordinates.Y, player.Coordinates.X] = room;
                player.Coordinates.Y--;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.DownArrow && player.Coordinates.Y < (rowNumber - 1))
            {
                map[player.Coordinates.Y, player.Coordinates.X] = room;
                player.Coordinates.Y++;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.LeftArrow && player.Coordinates.X > 0)
            {
                map[player.Coordinates.Y, player.Coordinates.X] = room;
                player.Coordinates.X--;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.RightArrow && player.Coordinates.X < (columnNumber - 1))
            {
                map[player.Coordinates.Y, player.Coordinates.X] = room;
                player.Coordinates.X++;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Z)
            {
                player.IsAlife = false;
            }
            return false;

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
            Weapon = new SwordWeapon();
        }

        public IWeapon Weapon { get; set; }
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
            //return "[ ]";
        }
    }

    public class Pit : GameObject
    {
        public override string Render()
        {
            return "[O]";
            //return "[ ]";
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

    public enum WeaponType
    {
        Sword = 0,
        Bow = 1,
        MorningStar = 2
    }

    public interface IWeapon
    {
        bool Attack(Player player, Wumpus wumpus, int rowNumber, int columnNumber, ConsoleKeyInfo keyInfo);
        void ShowEquipement();
    }

    public class SwordWeapon : IWeapon
    {
        public bool Attack(Player player, Wumpus wumpus, int rowNumber, int columnNumber, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.W && player.Coordinates.Y > 0)
            {
                if (wumpus.Coordinates.Y == player.Coordinates.Y - 1 && wumpus.Coordinates.X == player.Coordinates.X)
                {
                    wumpus.IsAlife = false;
                }
                return true;
            }

            if (keyInfo.Key == ConsoleKey.S && player.Coordinates.Y < (rowNumber - 1))
            {
                if (wumpus.Coordinates.Y == player.Coordinates.Y + 1 && wumpus.Coordinates.X == player.Coordinates.X)
                {
                    wumpus.IsAlife = false;
                }
                return true;
            }

            if (keyInfo.Key == ConsoleKey.D && player.Coordinates.X < (columnNumber - 1))
            {
                if (wumpus.Coordinates.X == player.Coordinates.X + 1 && wumpus.Coordinates.Y == player.Coordinates.Y)
                {
                    wumpus.IsAlife = false;
                }
                return true;
            }

            if (keyInfo.Key == ConsoleKey.A && player.Coordinates.X > 0)
            {
                if (wumpus.Coordinates.X == player.Coordinates.X - 1 && wumpus.Coordinates.Y == player.Coordinates.Y)
                {
                    wumpus.IsAlife = false;
                }
                return true;
            }
            return false;
        }

        public void ShowEquipement()
        {
            Console.WriteLine("You equiped sword");
        }
    }
    public class BowWeapon : IWeapon
    {
        public BowWeapon()
        {
            ArrowsCount = 3;
        }
        public int ArrowsCount { get; private set; }
        public bool Attack(Player player, Wumpus wumpus, int rowNumber, int columnNumber, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.W && player.Coordinates.Y > 0 && ArrowsCount > 0)
            {
                if (wumpus.Coordinates.Y < player.Coordinates.Y && wumpus.Coordinates.X == player.Coordinates.X)
                {
                    wumpus.IsAlife = false;
                }
                ArrowsCount = ArrowsCount - 1;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.S && player.Coordinates.Y < (rowNumber - 1) && ArrowsCount > 0)
            {
                if (wumpus.Coordinates.Y > player.Coordinates.Y && wumpus.Coordinates.X == player.Coordinates.X)
                {
                    wumpus.IsAlife = false;
                }
                ArrowsCount = ArrowsCount - 1;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.D && player.Coordinates.X < (columnNumber - 1) && ArrowsCount > 0)
            {
                if (wumpus.Coordinates.X > player.Coordinates.X && wumpus.Coordinates.Y == player.Coordinates.Y)
                {
                    wumpus.IsAlife = false;
                }
                ArrowsCount = ArrowsCount - 1;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.A && player.Coordinates.X > 0 && ArrowsCount > 0)
            {
                if (wumpus.Coordinates.X < player.Coordinates.X && wumpus.Coordinates.Y == player.Coordinates.Y)
                {
                    wumpus.IsAlife = false;
                }
                ArrowsCount = ArrowsCount - 1;
                return true;
            }
            return false;
        }

        public void ShowEquipement()
        {
            Console.WriteLine("You equiped bow");
            Console.WriteLine("Arrows left:" + ArrowsCount);
            if (ArrowsCount == 0)
            {
                Console.WriteLine("You run out of arrows");
            }
        }
    }
    public class MorningStarWeapon : IWeapon
    {
        public MorningStarWeapon()
        {
            UsesLeft = 2;
        }

        public int UsesLeft { get; private set; }
        public bool Attack(Player player, Wumpus wumpus, int rowNumber, int columnNumber, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.W && player.Coordinates.Y > 0 && UsesLeft > 0)
            {
                if (wumpus.Coordinates.Y == player.Coordinates.Y - 2 && wumpus.Coordinates.X == player.Coordinates.X)
                {
                    wumpus.IsAlife = false;
                }
                UsesLeft = UsesLeft - 1;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.S && player.Coordinates.Y < (rowNumber - 1) && UsesLeft > 0)
            {
                if (wumpus.Coordinates.Y == player.Coordinates.Y + 2 && wumpus.Coordinates.X == player.Coordinates.X)
                {
                    wumpus.IsAlife = false;
                }
                UsesLeft = UsesLeft - 1;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.D && player.Coordinates.X < (columnNumber - 1) && UsesLeft > 0)
            {
                if (wumpus.Coordinates.X == player.Coordinates.X + 2 && wumpus.Coordinates.Y == player.Coordinates.Y)
                {
                    wumpus.IsAlife = false;
                }
                UsesLeft = UsesLeft - 1;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.A && player.Coordinates.X > 0 && UsesLeft > 0)
            {
                if (wumpus.Coordinates.X == player.Coordinates.X - 2 && wumpus.Coordinates.Y == player.Coordinates.Y)
                {
                    wumpus.IsAlife = false;
                }
                UsesLeft = UsesLeft - 1;
                return true;
            }
            return false;
        }

        public void ShowEquipement()
        {
            Console.WriteLine("You equiped MorningStar");
            Console.WriteLine("Uses left:" + UsesLeft);
            if (UsesLeft == 0)
            {
                Console.WriteLine("The MorningStar is broken");
            }
        }
    }

}
