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

using System;
using OpenTK;
using OpenTK.Input;

namespace MistwoodTales.Game.Client.Systems.Rendering.ConsoleGL
{
    public class CGLMouse
    {
        /// <summary>
        /// Gets the X coordinate of the mouse, in characters.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the Y coordinate of the mouse, in characters.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Indicator whether the left mouse button is currently pressed down.
        /// </summary>
        public bool LeftPressed { get; private set; }

        /// <summary>
        /// Indicator whether the right mouse button is currently pressed down.
        /// </summary>
        public bool RightPressed { get; private set; }

        private bool rightClick;
        private bool leftClick;
        private int charWidth;
        private int charHeight;
        private float scale;
        private int offsetX;
        private int offsetY;

        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseMoveEventArgs> MouseMove;

        internal CGLMouse(GameWindow window)
        {
           
            window.Mouse.Move += window_MouseMove;
            window.Mouse.ButtonDown += window_MouseDown;
            window.Mouse.ButtonUp += window_MouseUp;
        }

        internal void Calibrate(int charWidth, int charHeight, int offsetX, int offsetY, float scale)
        {
            this.charWidth = charWidth;
            this.charHeight = charHeight;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.scale = scale;
        }

        private void window_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                LeftPressed = false;
                leftClick = true;
            }
            else if (e.Button == OpenTK.Input.MouseButton.Right)
            {
                RightPressed = false;
                rightClick = true;
            }
            MouseUp?.Invoke(sender, e);
        }

        private void window_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                LeftPressed = true;
                leftClick = false;
            }
            else if (e.Button == OpenTK.Input.MouseButton.Right)
            {
                RightPressed = true;
                rightClick = false;
            }
            MouseDown?.Invoke(sender, e);
        }

        private void window_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            X = (int)((e.X - offsetX) / (charWidth * scale));
            Y = (int)((e.Y - offsetY) / (charHeight * scale));
            rightClick = false;
            leftClick = false;
            MouseMove?.Invoke(sender, e);
        }

        /// <summary>
        /// Gets whether the right mouse button was clicked.
        /// </summary>
        /// <returns></returns>
        public bool GetRightClick()
        {
            if (rightClick)
            {
                rightClick = false;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Gets whether the left mouse button was clicked.
        /// </summary>
        /// <returns></returns>
        public bool GetLeftClick()
        {
            if (leftClick)
            {
                leftClick = false;
                return true;
            }
            else return false;
        }
    }
}
