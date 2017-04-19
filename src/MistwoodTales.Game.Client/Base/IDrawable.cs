using MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL;
using RogueSharp;

namespace MistwoodTales.Game.Client.Base
{
    interface IDrawable
    {

        CGLColor Color { get; set; }
        char Symbol { get; set; }
        int X { get; set; }
        int Y { get; set; }

        void Draw(CGLConsole console, IMap map);
    }
}
