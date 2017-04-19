using System.Collections.Generic;
using MistwoodTales.Game.Client.Base;
using MistwoodTales.Game.Client.Scheduling;
using MistwoodTales.Game.Client.Systems;
using MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL;
using RogueSharp;

namespace MistwoodTales.Game.Client.Entities
{
    public class Actor : IActor, IDrawable, IScheduleable
    {
        public Actor()
        {
            Behaviors = new List<IBehavior>();
        }

        public string Name { get; set; }
        public int LightRadius { get; set; }
        public CGLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(CGLConsole console, IMap map)
        {

            // Don't draw actors in cells that haven't been explored
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            // Only draw the actor with the color and symbol when they are in field-of-view
            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }
        }

        public int Attack { get; set; }
        public int AttackChance { get; set; }
        public int Defense { get; set; }
        public int DefenseChance { get; set; }
        public int Gold { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Speed { get; set; }
        public int Time => Speed;


        public virtual void PerformAction(CommandSystem commandSystem)
        {
            foreach (var behavior in Behaviors)
            {
                behavior.Act(commandSystem);
            }
        }

        public ICollection<IBehavior> Behaviors { get; }

        //public bool Move(Direction direction)
        //{
        //    int x=0, y=0;
        //    switch (direction)
        //    {
        //        case Direction.Up:
        //        {
        //            y = Y - 1;
        //            break;
        //        }
        //        case Direction.Down:
        //        {
        //            y = Y + 1;
        //            break;
        //        }
        //        case Direction.Left:
        //        {
        //            x = X - 1;
        //            break;
        //        }
        //        case Direction.Right:
        //        {
        //            x = X + 1;
        //            break;
        //        }
        //    }
        //    return Game.CurrentMap.SetActorPosition(this, x, y);
        //}
    }
}
