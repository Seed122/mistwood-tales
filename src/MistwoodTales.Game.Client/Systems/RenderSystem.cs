using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MistwoodTales.Game.Client.Base;
using MistwoodTales.Game.Client.Scheduling;
using MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL;

namespace MistwoodTales.Game.Client.Systems
{
    public class RenderSystem
    {

        // The screen height and width are in number of tiles
        private readonly int _screenWidth = Configuration.ScreenWidth;
        private readonly int _screenHeight = Configuration.ScreenHeight;
        private CGLRootConsole _rootConsole;


        // Below the map console is the message console which displays attack rolls and other information
        private readonly int _messageWidth = Configuration.ScreenWidth;
        private readonly int _messageHeight = Configuration.MessagesFrameHeight;
        private CGLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private readonly int _statWidth = Configuration.StatFrameWidth;
        private readonly int _statHeight = Configuration.ScreenHeight;
        private CGLConsole _statConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private readonly int _mapWidth = Configuration.ScreenWidth - Configuration.StatFrameWidth;
        private readonly int _mapHeight = Configuration.ScreenHeight - Configuration.MessagesFrameHeight;
        private CGLConsole _mapConsole;

        private readonly string _fontFileName = Configuration.RenderingFontFileName;
        private readonly string _consoleTitle = "MistwoodTales";

        public void Start()
        {
            // This must be the exact name of the bitmap font file we are using or it will error.
            // The title will appear at the top of the console window
            CGLSettings s = new CGLSettings
            {
                StartWindowState = CGLWindowState.Fullscreen,
                BitmapFile = _fontFileName,
                Scale = 1.5f,
                Width = _screenWidth,
                Height = _screenHeight,
                Title = _consoleTitle,
                WindowBorder = CGLWindowBorder.Fixed,
            };

            _rootConsole = new CGLRootConsole(s);
            _mapConsole = new CGLConsole(_mapWidth, _mapHeight);
            _messageConsole = new CGLConsole(_messageWidth, _messageHeight);
            _statConsole = new CGLConsole(_statWidth, _statHeight);
            //_inventoryConsole = new CGLConsole(_inventoryWidth, _inventoryHeight);
            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;
            try
            {
                if (_scheduleMode == ScheduleMode.Timer)
                {
                    _scheduleTimer = new Timer(ScheduleTimerCallback, null, 0, _scheduleTimerPeriod);
                    // starts automatically
                }
                RedrawNeeded = true;
                _rootConsole.Run(Configuration.FramesPerSecond, Configuration.InputUpdatesPerSecond);
            }
            finally
            {
                _scheduleTimer?.Dispose();
            }
        }



        // Event handler for RLNET's Update event
        private void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            var keyPress = _rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                switch (keyPress.Key)
                {
                    case CGLKey.Escape:
                        _rootConsole.Close();
                        break;
                    case CGLKey.Space:
                        if (_scheduleMode == ScheduleMode.Manual)
                        {
                            Game.CommandSystem.Act();
                        }
                        break;
                }
            }

            bool leftClicked = _rootConsole.Mouse.GetLeftClick();
            if (leftClicked)
            {
                var mouseX = _rootConsole.Mouse.X;
                var mouseY = _rootConsole.Mouse.Y;
                var mobs = Game.CurrentMap.GetMonstersAt(mouseX, mouseY);
                if (mobs.Any())
                    Game.CommandSystem.Attack(Game.Player, mobs.First());
                else
                    Game.CommandSystem.SetPlayerPath(mouseX, mouseY);
            }
        }



        // Event handler for RLNET's Render event
        private void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (!RedrawNeeded)
                return;
            //Game.MessageLog.Draw(_messageConsole);
            Game.CurrentMap.Draw(_mapConsole, _statConsole);
            Game.Player.Draw(_mapConsole, Game.CurrentMap);
            //Game.Player.DrawStats(_statConsole);
            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            CGLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, 0);
            //CGLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
            //CGLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
            _rootConsole.Draw();
            RedrawNeeded = false;
        }


        private static void ScheduleTimerCallback(object state)
        {
            Game.CommandSystem.Act();
        }

        private Timer _scheduleTimer;


        private readonly int _scheduleTimerPeriod = Configuration.ScheduleTimerPeriod;
        private readonly ScheduleMode _scheduleMode = Configuration.ScheduleMode;

        public bool RedrawNeeded { get; set; }

    }
}
