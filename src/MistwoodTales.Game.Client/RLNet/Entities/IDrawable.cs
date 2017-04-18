using ConsoleGL;
using RogueSharp;

namespace MistwoodTales.Game.Client.RLNet.Entities
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
