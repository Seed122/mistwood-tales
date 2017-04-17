using System;
using System.Threading.Tasks;
using Microsoft.GotDotNet;

namespace MistwoodTales.Game.Client.Legacy
{
    static class Camera
    {
        private static int _screenCenterX;
        private static int _screenCenterY;
        private static int _camX1;
        private static int _camY1;
        private static int _camX2;
        private static int _camY2;

        public static void Init(int width, int height)
        {
            Width = width;
            Height = height;
            _screenCenterX = Width / 2;
            _screenCenterY = Height / 2;

            // create player panel
            int ppwidth = 40;
            int ppheight = 4;
            _currentItems = new MapItem[Width,Height];
        }

        public const int RENDER_DELAY = 100;

        private static async void DrawMap()
        {
            //throw new NotImplementedException();
        }

        private static MapItem[,] _currentItems;

        public static int Height { get; private set; }
               
        public static int Width { get; private set; }

        public static async void Render()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    ConsoleEx.Move(Console.WindowWidth - 1, Console.WindowHeight - 1);
                    UpdatePosition();
                    CalculateLandscape();
                    //RenderInternal();
                    //RenderNPCs();
                    RenderPlayer();
                    //RenderPlayerPanel();
                    //RenderSightingsPanel();
                    await Task.Delay(RENDER_DELAY);
                }
            });
        }

        private static void UpdatePosition()
        {

            Player player = World.Instance.Player;
            Map map = World.Instance.Map;
            _camX1 = (player.Point.X - _screenCenterX);
            if (_camX1 < 0)
            {
                _camX1 = 0;
            }
            _camY1 = (player.Point.Y - _screenCenterY);
            if (_camY1 < 0)
            {
                _camY1 = 0;
            }
            _camX2 = _camX1 + Width;
            if (_camX2 >= map.Width)
            {
                _camX2 = map.Width;
                _camX1 = _camX2 - Width;
            }
            _camY2 = _camY1 + Height;
            if (_camY2 >= map.Height)
            {
                _camY2 = map.Height;
                _camY1 = _camY2 - Height;
            }
        }

        //private static void RenderInternal()
        //{
        //    for (int i = 0; i < Height; i++)
        //    {
        //        for (int j = 0; j < Width; j++)
        //        {

        //        }
        //    }
        //}

        private static void RenderSightingsPanel()
        {
            
        }

        private static void RenderPlayerPanel()
        {
        }

        private static void RenderPlayer()
        {
            //SetColor(ConsoleForeground.DarkGreen, ConsoleBackground.White);
            Player player = World.Instance.Player;
            char symbol;
            switch (player.FaceDirection)
            {
                case Direction.Right:
                    symbol = '→';
                    break;
                case Direction.Left:
                    symbol = '←';
                    break;
                case Direction.Up:
                    symbol = '↑';
                    break;
                case Direction.Down:
                    symbol = '↓';
                    break;
                default:
                    symbol = ' ';
                    break;
            }
            var coords = AbsoluteToRelativeCoords(player.Point);
            ConsoleEx.WriteAt(coords.X, coords.Y, symbol.ToString());
        }

        private static void RenderNPCs()
        {
        }

        //private static ColorPair _lastColor = new ColorPair();

        //private static void SetColor(ConsoleForeground foreground, ConsoleBackground background)
        //{
        //    if(foreground == _lastColor.Foreground && background == _lastColor.Background)
        //        return;
        //    ConsoleEx.TextColor(foreground, background);
        //}

        private static void CalculateLandscape()
        {

            Map map = World.Instance.Map;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    // optimization for player panel
                    // do not render map there
                    //if ((j >= PlayerPanel.wstartx)
                    //    && (j < PlayerPanel.wendx)
                    //    && (i >= PlayerPanel.wstarty)
                    //    && (i < PlayerPanel.wendy))
                    //{
                    //    continue; ;
                    //}
                    var coords = RelativeToAbsoluteCoords(new Point(j, i));
                    MapItem item = map.GetItem(coords);
                    //switch (item.Symbol)
                    //{
                    //    case '≈':
                    //        SetColor(ConsoleForeground.Blue, ConsoleBackground.Aquamarine);
                    //        break;
                    //    default:
                    //        SetColor(ConsoleForeground.White, ConsoleBackground.Black);
                    //        break;
                    //}
                    if (!_currentItems[j, i].Equals(item))
                    {
                        _currentItems[j, i] = item;
                        ConsoleEx.WriteAt(j, i, item.Symbol.ToString());
                    }
                    //SetColor(item.Color);
                }
            }
        }

        private static Point AbsoluteToRelativeCoords(Point absoluteCoords)
        {
            return new Point(absoluteCoords.X - _camX1, absoluteCoords.Y - _camY1);
        }

        private static Point RelativeToAbsoluteCoords(Point relativeCoords)
        {
            return new Point(relativeCoords.X + _camX1, relativeCoords.Y + _camY1);
        }
    }


}
