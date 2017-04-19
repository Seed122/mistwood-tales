using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistwoodTales.Util.MapTransformer
{
    struct MapItem
    {
        public char Symbol { get; set; }
        public short ForegroundRed { get; set; }
        public short ForegroundGreen { get; set; }
        public short ForegroundBlue { get; set; }
        public short BackgroundRed { get; set; }
        public short BackgroundGreen { get; set; }
        public short BackgroundBlue { get; set; }
        public bool CanWalk { get; set; }
    }

    class Program
    {
        private static void Main(string[] args)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string mapFilepath = Path.Combine(baseDir, @"..\..\..\MistwoodTales.Game\mistmap-ed.txt");
            string resultFilepath = Path.Combine(baseDir, @"..\..\..\MistwoodTales.Game\map.txt");
            var res = new List<MapItem>();
            int width = 0, height = 0;
            using (var inFile = File.OpenRead(mapFilepath))
            using (StreamReader sr = new StreamReader(inFile, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    //if (line == null)
                    //    continue;
                    if (width == 0)
                        width = line.Length;
                    foreach (var ch in line)
                    {
                        var item = new MapItem() { Symbol = ch };
                        switch (ch)
                        {
                            case '≈':
                                item.CanWalk = false;
                                item.ForegroundRed = 102;
                                item.ForegroundGreen = 217;
                                item.ForegroundBlue = 255;
                                item.BackgroundRed = 10;
                                item.BackgroundGreen = 109;
                                item.BackgroundBlue = 255;
                                break;
                            case '`':
                                item.CanWalk = true;
                                item.ForegroundRed = 255;
                                item.ForegroundGreen = 238;
                                item.ForegroundBlue = 117;
                                break;
                            case ':':
                                item.CanWalk = true;
                                item.ForegroundRed = 255;
                                item.ForegroundGreen = 242;
                                item.ForegroundBlue = 37;
                                break;
                            case '/':
                                item.CanWalk = false;
                                item.ForegroundRed = 140;
                                item.ForegroundGreen = 82;
                                item.ForegroundBlue = 80;
                                break;
                            default:
                                break;
                        }
                        res.Add(item);
                    }
                    height++;
                }
            }
            using (var outFile = File.OpenWrite(resultFilepath))
            using (StreamWriter sw = new StreamWriter(outFile, Encoding.UTF8))
            {
                sw.WriteLine($"{width},{height}");
                foreach (var item in res)
                {
                    var line = $" ,{item.ForegroundRed},{item.ForegroundGreen},{item.ForegroundBlue}, ,{item.BackgroundRed},{item.BackgroundGreen},{item.BackgroundBlue}" +
                        $", ,{item.Symbol},{(item.CanWalk ? '1' : '0')}";
                    sw.WriteLine(line);
                }
            }
        }
    }
}
