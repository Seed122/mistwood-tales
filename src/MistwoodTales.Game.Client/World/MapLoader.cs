using System.IO;

namespace MistwoodTales.Game.Client.World
{
    public static class MapLoader
    {
        public static MistwoodMap Load(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            using (var reader = new StreamReader(fs))
            {
                var res = new LocationMap();
                
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return null;
                {
                    var parts = line.Split(',');
                    int width = int.Parse(parts[0]);
                    int height = int.Parse(parts[1]);
                    res.Initialize(width, height);
                }
                int j = 0, i = 0;
                while (i < res.Width && j < res.Height)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        return null;
                    var parts = line.Split(',');

                    var symbol = parts[9][0];
                    var canWalk = int.Parse(parts[10]) == 1;
                    res.SetCellProperties(i, j, true, canWalk);
                    //Cell t;
                    ++i;
                    j += i / res.Width;
                    i = i % res.Width;

                }
                return res;
            }
        }
    }
}
