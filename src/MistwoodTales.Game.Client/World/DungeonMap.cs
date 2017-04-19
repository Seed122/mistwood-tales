using System.Collections.Generic;
using System.Linq;
using MistwoodTales.Game.Client.Entities;
using MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL;
using RogueSharp;

namespace MistwoodTales.Game.Client.World
{
    public class DungeonMap : MistwoodMap
    {
        public DungeonMap()
        {
            // Initialize the list of rooms when we create a new DungeonMap
            Rooms = new List<Rectangle>();
            Doors = new List<Door>();
        }

        public List<Rectangle> Rooms { get; set; }
        public List<Door> Doors { get; set; }
        
        // Return the door at the x,y position or null if one is not found.
        public Door GetDoor(int x, int y)
        {
            return Doors.SingleOrDefault(d => d.X == x && d.Y == y);
        }

        public override void Draw(CGLConsole mapConsole, CGLConsole statConsole)
        {
            base.Draw(mapConsole, statConsole);
            foreach (Door door in Doors)
            {
     //           door.Draw(mapConsole, this);
            }
        }

        // The actor opens the door located at the x,y position
        private void OpenDoor(Actor actor, int x, int y)
        {
            Door door = GetDoor(x, y);
            if (door != null && !door.IsOpen)
            {
                door.IsOpen = true;
                var cell = GetCell(x, y);
                // Once the door is opened it should be marked as transparent and no longer block field-of-view
                SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);

                //Game.MessageLog.Add($"{actor.Name} opened a door");
            }
        }

        // Look for a random location in the room that is walkable.
        public Point GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }

            // If we didn't find a walkable location in the room return null
            throw new PathNotFoundException();
        }

        // Iterate through each Cell in the room and return true if any are walkable
        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Returns true when able to place the Actor on the cell or false otherwise
        public override bool SetActorPosition(Actor actor, int x, int y)
        {
            var res = base.SetActorPosition(actor, x, y);
            if (!res)
            {
                OpenDoor(actor, x, y);
            }
            return res;
        }
    }
}
