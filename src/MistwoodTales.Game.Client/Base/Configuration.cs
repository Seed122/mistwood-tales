using System;
using System.Collections.Generic;
using System.Text;

namespace MistwoodTales.Game.Client.Base
{
    public static class Configuration
    {
        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
        
        public static int StatFrameWidth { get; set; }

        public static int MessagesFrameHeight { get; set; }

        public static int FramesPerSecond { get; set; }
        public static int InputUpdatesPerSecond { get; set; }
    }
}
