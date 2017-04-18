using System;
using System.Linq;
using System.Timers;
using MistwoodTales.Game.Client.Legacy;
using MistwoodTales.Game.Client.RLNet.Base;
using MistwoodTales.Game.Client.RLNet.Scheduling;
using MistwoodTales.Game.Client.RLNet.Systems;
using MistwoodTales.Game.Client.RLNet.World;
using RLNET;
using RogueSharp.Random;
using Player = MistwoodTales.Game.Client.RLNet.Entities.Player;

namespace MistwoodTales.Game.Client
{
    class Game
    {
        private static Map _map;


        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 120;
        private static readonly int _screenHeight = 80;
        private static RLRootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 100;
        private static readonly int _mapHeight = 69;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 100;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 80;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        //private static readonly int _inventoryWidth = 80;
        //private static readonly int _inventoryHeight = 11;
        //private static RLConsole _inventoryConsole;

        //private static bool _renderRequired = true;

        public static CommandSystem CommandSystem { get; private set; }

        public static Player Player { get; set; }
        public static MessageLog MessageLog { get; private set; }
        static void Main(string[] args)
        {

            // Establish the seed for the random number generator from the current time
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";
            // The title will appear at the top of the console window
            string consoleTitle = "RougeSharp V3 Tutorial - Level 1";
            
            CommandSystem = new CommandSystem();
            SchedulingSystem = new SchedulingSystem();
            // The next two lines already existed

            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            //_rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1.5f, consoleTitle);

            RLSettings s = new RLSettings
            {
                StartWindowState = RLWindowState.Fullscreen,
                BitmapFile = fontFileName,
                Scale = 1.5f,
                Width = _screenWidth,
                Height = _screenHeight,
                Title = consoleTitle,
                WindowBorder = RLWindowBorder.Fixed,
            };

            _rootConsole = new RLRootConsole(s);

            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            //_inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);
            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;
            // Begin RLNET's game loop
            Player = new Player();
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7);

            //CurrentMap = mapGenerator.CreateMap();
            CurrentMap = MapLoader.Load(@"..\..\..\MistwoodTales.Game\map.txt");

            CurrentMap.AddPlayer(Player);
            CurrentMap.UpdatePlayerFieldOfView();
            
            // Create a new MessageLog and print the random seed used to generate the level
            MessageLog = new MessageLog();


            if (_scheduleMode == ScheduleMode.Timer)
            {
                _scheduleTimer = new Timer(150);
                _scheduleTimer.Elapsed += (o, e) => {
                    CommandSystem.Act();
                };
                _scheduleTimer.Start();
            }

            _rootConsole.Run(10);
            CommandSystem.RedrawNeeded = true;
        }

        public static DotNetRandom Random { get; set; }

        public static MistwoodMap CurrentMap { get; set; }
        public static SchedulingSystem SchedulingSystem { get; private set; }

        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                switch (keyPress.Key)
                {
                    //case RLKey.Up:
                    //    CommandSystem.MovePlayer(Direction.Up);
                    //    break;
                    //case RLKey.Down:
                    //    CommandSystem.MovePlayer(Direction.Down);
                    //    break;
                    //case RLKey.Left:
                    //    CommandSystem.MovePlayer(Direction.Left);
                    //    break;
                    //case RLKey.Right:
                    //    CommandSystem.MovePlayer(Direction.Right);
                    //    break;
                    case RLKey.Escape:
                        _rootConsole.Close();
                        break;
                    case RLKey.Space:
                        if (_scheduleMode == ScheduleMode.Manual)
                        {
                            CommandSystem.Act();
                        }
                        break;
                }
            }

            bool leftClicked = _rootConsole.Mouse.LeftPressed;
            if (leftClicked)
            {
                var mouseX = _rootConsole.Mouse.X;
                var mouseY = _rootConsole.Mouse.Y;
                var mobs = CurrentMap.GetMonstersAt(mouseX, mouseY);
                if (mobs.Any())
                    CommandSystem.Attack(Player, mobs.First());
                else
                    CommandSystem.SetPlayerPath(mouseX, mouseY);
            }
        }

        private static int _steps = 0;
        private static ScheduleMode _scheduleMode = ScheduleMode.Timer;
        private static System.Timers.Timer _scheduleTimer; 

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            //if (!CommandSystem.RedrawNeeded)
                //return;

            _mapConsole.Clear();
            _statConsole.Clear();
            _messageConsole.Clear();

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);


            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            _statConsole.Print(1, 1, "Stats", Colors.TextHeading);
            //_inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            //_inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);
            MessageLog.Draw(_messageConsole);
            CurrentMap.Draw(_mapConsole, _statConsole);
            Player.Draw(_mapConsole, CurrentMap);
            Player.DrawStats(_statConsole);
            // Blit the sub consoles to the root console in the correct locations
            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, 0);
            RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
            RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0,
                _screenHeight - _messageHeight);
            //RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);
            // Tell RLNET to draw the console that we set
            _rootConsole.Draw();
            CommandSystem.RedrawNeeded = false;
        }

        //static void Main2(string[] args)
        //{
        //    Console.WindowHeight = 40;
        //    Console.WindowWidth = 100;
        //    Console.OutputEncoding = System.Text.Encoding.UTF8;
        //    Camera.Init(Console.WindowWidth, Console.WindowHeight);
        //    Camera.Render();

        //    //ProcessInput();

        //    //ConsoleEx.TextColor(ConsoleForeground.White, ConsoleBackground.Black);
        //    //Microsoft.GotDotNet.ConsoleEx.DrawRectangle(BorderStyle.LineDouble, 6, 6, 30, 10, false);

        //    Player player = World.World.Instance.Player;
        //    while (true)
        //    {
        //        var key = Console.ReadKey(true);
        //        switch (key.Key)
        //        {
        //            case ConsoleKey.RightArrow:
        //                player.Move(Direction.Right);
        //                break;
        //            case ConsoleKey.LeftArrow:
        //                player.Move(Direction.Left);
        //                break;
        //            case ConsoleKey.UpArrow:
        //                player.Move(Direction.Up);
        //                break;
        //            case ConsoleKey.DownArrow:
        //                player.Move(Direction.Down);
        //                break;
        //        }
        //        Thread.Sleep(100);
        //    }
        //}

        
    }
}
