using System.Collections.Generic;
using System.IO;

namespace MistwoodTales.Game.Client.Legacy
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        private MapItem[,] _data;

        public Map(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            using (var reader = new StreamReader(fs))
            {
                var line = reader.ReadLine();
                if(string.IsNullOrEmpty(line))
                    return;
                {
                    var parts = line.Split(',');
                    Width = int.Parse(parts[0]);
                    Height = int.Parse(parts[1]);
                    _data = new MapItem[Height, Width];
                }
                int j = 0, i = 0;
                while (i<Width && j<Height)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        return;
                    var parts = line.Split(',');
                    var item = new MapItem();

                    item.Symbol = parts[9][0];
                    item.CanWalk = int.Parse(parts[10]) == 1;

                    _data[j, i] = item;
                    ++i;
                    j += i / Width;
                    i = i % Width;
                } 
            }
        }


        public bool CheckIfCoordsAreAvailable(int x, int y)
        {
            if (x < 0
                || y < 0
                || x >= Width
                || y >= Height)
                return false;
            return _data[y,x].CanWalk;
        }


        public MapItem GetItem(int x, int y)
        {
            if (x < 0
                || y < 0
                || x >= Width
                || y >= Height)
                throw new KeyNotFoundException();
            return _data[y, x];
        }

        public MapItem GetItem(Point point)
        {
            return GetItem(point.X, point.Y);
        }
    }
}
