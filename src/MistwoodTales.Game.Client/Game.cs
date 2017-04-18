using System;
using System.Linq;
using System.Threading;
using ConsoleGL;
using MistwoodTales.Game.Client.Base;
using MistwoodTales.Game.Client.Scheduling;
using MistwoodTales.Game.Client.Systems;
using MistwoodTales.Game.Client.World;
using RogueSharp;
using RogueSharp.Random;
using Player = MistwoodTales.Game.Client.Entities.Player;

namespace MistwoodTales.Game.Client
{
    class Game
    {
        public static CommandSystem CommandSystem { get; private set; }
        public static Player Player { get; set; }
        public static MessageLog MessageLog { get; private set; }

        private static void Main(string[] args)
        {
            InitSettings();
            // Establish the seed for the random number generator from the current time
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
            
            CommandSystem = new CommandSystem();
            SchedulingSystem = new SchedulingSystem();
            RenderSystem = new RenderSystem();
            Player = new Player();
            MapGenerator mapGenerator = new MapGenerator(Configuration.ScreenWidth, Configuration.ScreenHeight, 20, 13, 7);

            CurrentMap = mapGenerator.CreateMap();

            //CurrentMap = MapLoader.Load(@"..\..\..\MistwoodTales.Game\map.txt");
            //CurrentMap.AddPlayer(Player);

            CurrentMap.UpdatePlayerFieldOfView();
            
            // Create a new MessageLog and print the random seed used to generate the level
            MessageLog = new MessageLog();
            RenderSystem.Start();
            EmptyCycle();
        }

        private static void EmptyCycle()
        {
            while (true)
            {
                Thread.Sleep(2000);
            }
        }

        private static void InitSettings()
        {
            Configuration.ScreenWidth = 120;
            Configuration.ScreenHeight = 80;
            Configuration.MessagesFrameHeight = 11;
            Configuration.StatFrameWidth = 20;
            Configuration.FramesPerSecond = 100;
            Configuration.InputUpdatesPerSecond = 100;
        }

        public static DotNetRandom Random { get; set; }

        public static MistwoodMap CurrentMap { get; set; }
        public static SchedulingSystem SchedulingSystem { get; private set; }
        public static RenderSystem RenderSystem { get; set; }


    }
}
