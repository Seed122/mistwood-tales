﻿using System.Collections.Generic;
using System.Linq;
using MistwoodTales.Game.Client.Base;
using MistwoodTales.Game.Client.Entities;
using MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL;
using RogueSharp;

namespace MistwoodTales.Game.Client.World
{
    public class MistwoodMap: Map
    {

        protected readonly List<Actor> _actors;

        public MistwoodMap()
        {
            _actors = new List<Actor>();
        }

        public void AddMonster(Monster monster)
        {
            _actors.Add(monster);
            // After adding the monster to the map make sure to make the cell not walkable
            //SetIsWalkable(monster.X, monster.Y, false);

            Game.SchedulingSystem.Add(monster);
        }

        // The Draw method will be called each time the map is updated
        // It will render all of the symbols/colors for each cell to the map sub console
        public virtual void Draw(CGLConsole mapConsole, CGLConsole statConsole)
        {
            mapConsole.Clear();
            statConsole.Clear();
            foreach (ICell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
            // Iterate through each monster on the map and draw it after drawing the Cells
            // Keep an index so we know which position to draw monster stats at
            int i = 0;

            // Iterate through each monster on the map and draw it after drawing the Cells
            foreach (var actor in _actors)
            {
                switch (actor)
                {
                    case Monster monster:
                        // When the monster is in the field-of-view also draw their stats
                        if (IsInFov(monster.X, monster.Y))
                        {
                            monster.Draw(mapConsole, this);

                            // Pass in the index to DrawStats and increment it afterwards
                            monster.DrawStats(statConsole, i);
                            i++;
                        }
                        break;
                }
            }
        }




        // A helper method for setting the IsWalkable property on a Cell
        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            ICell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        public void RemoveMonster(Monster monster)
        {
            _actors.Remove(monster);
            // After removing the monster from the map, make sure the cell is walkable again
            //SetIsWalkable(monster.X, monster.Y, true);
            Game.SchedulingSystem.Remove(monster);
        }

        public IEnumerable<Monster> GetMonstersAt(int x, int y)
        {
            return _actors.Where(m => m.X == x && m.Y == y).OfType<Monster>();
        }

        // Returns true when able to place the Actor on the cell or false otherwise
        public virtual bool SetActorPosition(Actor actor, int x, int y)
        {
            if (actor.X == x && actor.Y == y)
                return false;
            // Only allow actor placement if the cell is walkable
            if (GetCell(x, y).IsWalkable)
            {
                // The cell the actor was previously on is now walkable
                //SetIsWalkable(actor.X, actor.Y, true);
                // Update the actor's position
                actor.X = x;
                actor.Y = y;
                // The new cell the actor is on is now not walkable
                //SetIsWalkable(x, y, false);
                // Don't forget to update the field of view if we just repositioned the player
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        // Called by MapGenerator after we generate a new map to add the player to the map
        public virtual void AddPlayer(Player player)
        {
            //SetIsWalkable(player.X, player.Y, false);
            _actors.Add(player);
            UpdatePlayerFieldOfView();
            Game.SchedulingSystem.Add(player);
        }

        // This method will be called any time we move the player to update field-of-view
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            // Compute the field-of-view based on the player's location and awareness
            ComputeFov(player.X, player.Y, player.LightRadius, true);

            // Mark all cells in field-of-view as having been explored
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        protected virtual void SetConsoleSymbolForCell(CGLConsole console, ICell cell)
        {

            // Randomly blinking map:
            // var chars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM[];',./{}<:>?!@#$%^&*()_+";
            // console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, chars[Game.Random.Next(chars.Length)]);
            // return;


            // When we haven't explored a cell yet, we don't want to draw anything
            if (!cell.IsExplored)
            {
                return;
            }

            // When a cell is currently in the field-of-view it should be drawn with ligher colors
            if (IsInFov(cell.X, cell.Y))
            {
                // Choose the symbol to draw based on if the cell is walkable or not
                // '.' for floor and '#' for walls
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            // When a cell is outside of the field of view draw it with darker colors
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }
    }
}
