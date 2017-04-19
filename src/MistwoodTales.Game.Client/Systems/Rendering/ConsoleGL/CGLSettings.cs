#region license
/* 
 * Released under the MIT License (MIT)
 * Copyright (c) 2014 Travis M. Clark
 * Copyright (c) 2017 Sergey Semenov
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 * DEALINGS IN THE SOFTWARE.
 */
#endregion

namespace MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL
{
    public class CGLSettings
    {
        public string BitmapFile { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CharWidth { get; set; }
        public int CharHeight { get; set; }
        public float Scale { get; set; }
        public string Title { get; set; }
        public CGLWindowState StartWindowState { get; set; }
        public CGLWindowBorder WindowBorder { get; set; }
        public CGLResizeType ResizeType { get; set; }

        public CGLSettings()
        {
            BitmapFile = "ascii_8x8.png";
            Width = 60;
            Height = 40;
            CharWidth = 8;
            CharHeight = 8;
            Scale = 1f;
            Title = "CGLNET Console";
            WindowBorder = CGLWindowBorder.Fixed;
            ResizeType = CGLResizeType.None;
            StartWindowState = CGLWindowState.Normal;
        }
    }
}
