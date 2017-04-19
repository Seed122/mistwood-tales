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
    /// <summary>
    /// Represents a color to be drawn
    /// </summary>
    public struct CGLColor
    {
        public float r;
        public float g;
        public float b;

        public static CGLColor Black { get; private set; }
        public static CGLColor Blue { get; private set; }
        public static CGLColor Green { get; private set; }
        public static CGLColor Cyan { get; private set; }
        public static CGLColor Red { get; private set; }
        public static CGLColor Magenta { get; private set; }
        public static CGLColor Brown { get; private set; }
        public static CGLColor LightGray { get; private set; }
        public static CGLColor Gray { get; private set; }
        public static CGLColor LightBlue { get; private set; }
        public static CGLColor LightGreen { get; private set; }
        public static CGLColor LightCyan { get; private set; }
        public static CGLColor LightRed { get; private set; }
        public static CGLColor LightMagenta { get; private set; }
        public static CGLColor Yellow { get; private set; }
        public static CGLColor White { get; private set; }
        public static CGLColor[] CGA { get; private set; }

        static CGLColor()
        {
            CGA = new CGLColor[16];
            CGA[0] = Black = new CGLColor(0, 0, 0);
            CGA[1] = Blue = new CGLColor(0, 0, 255);
            CGA[2] = Green = new CGLColor(0, 170, 0);
            CGA[3] = Cyan = new CGLColor(0, 170, 170);
            CGA[4] = Red = new CGLColor(170, 0, 0);
            CGA[5] = Magenta = new CGLColor(170, 0, 170);
            CGA[6] = Brown = new CGLColor(170, 85, 0);
            CGA[7] = LightGray = new CGLColor(170, 170, 170);
            CGA[8] = Gray = new CGLColor(85, 85, 85);
            CGA[9] = LightBlue = new CGLColor(85, 85, 255);
            CGA[10] = LightGreen = new CGLColor(85, 255, 85);
            CGA[11] = LightCyan = new CGLColor(85, 255, 255);
            CGA[12] = LightRed = new CGLColor(255, 85, 85);
            CGA[13] = LightMagenta = new CGLColor(255, 85, 255);
            CGA[14] = Yellow = new CGLColor(255, 255, 85);
            CGA[15] = White = new CGLColor(255, 255, 255);
        }

        public CGLColor(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public CGLColor(byte r, byte g, byte b)
        {
            this.r = (float)r / 255f;
            this.g = (float)g / 255f;
            this.b = (float)b / 255f;
        }

        internal OpenTK.Vector3 ToVector3()
        {
            return new OpenTK.Vector3(r, g, b);
        }

        /// <summary>
        /// Blends the two colors
        /// </summary>
        /// <param name="ColorA">Primary Color</param>
        /// <param name="ColorB">Secondary Color</param>
        /// <param name="Ratio">Ratio of the colors that are blended (255 - Full Primary, 0 - Full Secondary)</param>
        /// <returns>New Blended Color</returns>
        public static CGLColor Blend(CGLColor primary, CGLColor secondary, byte ratio)
        {
            return Blend(primary, secondary, (float)ratio / 255f);
        }

        /// <summary>
        /// Evenly blends two colors
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns>New Blended Color</returns>
        public static CGLColor Blend(CGLColor color1, CGLColor color2)
        {
            return Blend(color1, color2, .5f);
        }

        /// <summary>
        /// Blends the two colors
        /// </summary>
        /// <param name="ColorA">Primary Color</param>
        /// <param name="ColorB">Secondary Color</param>
        /// <param name="Ratio">Ratio of the colors that are blended (1f - Full Primary, 0f - Full Secondary)</param>
        /// <returns>New Blended Color</returns>
        public static CGLColor Blend(CGLColor primary, CGLColor secondary, float ratio)
        {
            return secondary - ((secondary - primary) * ratio);
        }

        public static CGLColor operator +(CGLColor color, float f)
        {
            return new CGLColor(color.r + f, color.g + f, color.b + f);
        }

        public static CGLColor operator -(CGLColor color, float f)
        {
            return new CGLColor(color.r - f, color.g - f, color.b - f);
        }

        public static CGLColor operator *(CGLColor color, float f)
        {
            return new CGLColor(color.r * f, color.g * f, color.b * f);
        }

        public static CGLColor operator /(CGLColor color, float f)
        {
            return new CGLColor(color.r / f, color.g / f, color.b / f);
        }

        public static CGLColor operator +(CGLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new CGLColor(color.r + f, color.g + f, color.b + f);
        }

        public static CGLColor operator -(CGLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new CGLColor(color.r - f, color.g - f, color.b - f);
        }

        public static CGLColor operator *(CGLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new CGLColor(color.r * f, color.g * f, color.b * f);
        }

        public static CGLColor operator /(CGLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new CGLColor(color.r / f, color.g / f, color.b / f);
        }

        public static CGLColor operator +(CGLColor colorA, CGLColor colorB)
        {
            return new CGLColor(colorA.r + colorB.r, colorA.g + colorB.g, colorA.b + colorB.b);
        }

        public static CGLColor operator -(CGLColor colorA, CGLColor colorB)
        {
            return new CGLColor(colorA.r - colorB.r, colorA.g - colorB.g, colorA.b - colorB.b);
        }

        public static CGLColor operator *(CGLColor colorA, CGLColor colorB)
        {
            return new CGLColor(colorA.r * colorB.r, colorA.g * colorB.g, colorA.b * colorB.b);
        }

        public static CGLColor operator /(CGLColor colorA, CGLColor colorB)
        {
            return new CGLColor(colorA.r / colorB.r, colorA.g / colorB.g, colorA.b / colorB.b);
        }

        public override string ToString()
        {
            return string.Format("R:{0}, B:{1}, G:{2}", r, g, b);
        }
    }
}
