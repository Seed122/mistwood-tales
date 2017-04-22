using System;
using System.Collections.Generic;
using System.Text;
using MistwoodTales.Game.Client.Scheduling;

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
        public static ScheduleMode ScheduleMode { get; set; }
        public static int ScheduleTimerPeriod { get; set; }
        public static string RenderingFontFileName { get; set; }
    }
}
