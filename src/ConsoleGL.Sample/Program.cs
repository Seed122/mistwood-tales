using System;

namespace ConsoleGL.Sample
{
    class Program
    {
        private static int playerX = 25;
        private static int playerY = 25;
        private static CGLRootConsole rootConsole;

        public static void Main()
        {
            CGLSettings settings = new CGLSettings();
            settings.BitmapFile = "ascii_8x8.png";
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = 60;
            settings.Height = 40;
            settings.Scale = 1f;
            settings.Title = "CGLNET Sample";
            settings.WindowBorder = CGLWindowBorder.Resizable;
            settings.ResizeType = CGLResizeType.ResizeCells;
            settings.StartWindowState = CGLWindowState.Normal;

            rootConsole = new CGLRootConsole(settings);
            rootConsole.Update += rootConsole_Update;
            rootConsole.Render += rootConsole_Render;
            rootConsole.OnLoad += rootConsole_OnLoad;
            rootConsole.Run();

        }

        static void rootConsole_OnLoad(object sender, EventArgs e)
        {
            //rootConsole.SetWindowState(CGLWindowState.Fullscreen);
        }

        static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            CGLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (keyPress.Key == CGLKey.Up)
                    playerY--;
                else if (keyPress.Key == CGLKey.Down)
                    playerY++;
                else if (keyPress.Key == CGLKey.Left)
                    playerX--;
                else if (keyPress.Key == CGLKey.Right)
                    playerX++;
                if (keyPress.Key == CGLKey.Escape)
                    rootConsole.Close();
            }
        }

        static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();

            rootConsole.Print(1, 1, "Hello World!", CGLColor.White);

            rootConsole.SetChar(playerX, playerY, '@');

            int color = 1;
            if (rootConsole.Mouse.LeftPressed)
            {
                color = 4;
            }
            rootConsole.SetBackColor(rootConsole.Mouse.X, rootConsole.Mouse.Y, CGLColor.CGA[color]);

            rootConsole.Draw();
        }
    }
}