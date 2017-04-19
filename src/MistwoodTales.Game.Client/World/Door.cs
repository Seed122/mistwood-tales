using System;
using System.Collections.Generic;
using System.Text;
using MistwoodTales.Game.Client.Base;
using MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL;
using RogueSharp;

namespace MistwoodTales.Game.Client.World
{
    public class Door: IDrawable
    {
        public Door()
        {
            Symbol = '+';
            Color = Colors.Door;
            BackgroundColor = Colors.DoorBackground;
        }
        public bool IsOpen { get; set; }

        public CGLColor Color { get; set; }
        public CGLColor BackgroundColor { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        
        public void Draw(CGLConsole console, IMap map)
        {
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            Symbol = IsOpen ? '-' : '+';
            if (map.IsInFov(X, Y))
            {
                Color = Colors.DoorFov;
                BackgroundColor = Colors.DoorBackgroundFov;
            }
            else
            {
                Color = Colors.Door;
                BackgroundColor = Colors.DoorBackground;
            }

            console.Set(X, Y, Color, BackgroundColor, Symbol);
        }
    }
}
