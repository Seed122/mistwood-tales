#region license
/* 
 * Released under the MIT License (MIT)
 * Copyright (c) 2014 Travis M. Clark
 * Copyright (c) 2017 Sergey Semenov
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


namespace ConsoleGL
{
    public class CGLKeyPress
    {
        public CGLKey Key { get; private set; }
        public bool Alt { get; private set; }
        public bool Shift { get; private set; }
        public bool Control { get; private set; }
        public bool Repeating { get; private set; }
        public bool NumLock { get; private set; }
        public bool CapsLock { get; private set; }
        public bool ScrollLock { get; private set; }
        public char? Char
        {
            get
            {
                switch (Key)
                {
                    case CGLKey.A: return Shift ^ CapsLock ? 'A' : 'a';
                    case CGLKey.B: return Shift ^ CapsLock ? 'B' : 'b';
                    case CGLKey.C: return Shift ^ CapsLock ? 'C' : 'c';
                    case CGLKey.D: return Shift ^ CapsLock ? 'D' : 'd';
                    case CGLKey.E: return Shift ^ CapsLock ? 'E' : 'e';
                    case CGLKey.F: return Shift ^ CapsLock ? 'F' : 'f';
                    case CGLKey.G: return Shift ^ CapsLock ? 'G' : 'g';
                    case CGLKey.H: return Shift ^ CapsLock ? 'H' : 'h';
                    case CGLKey.I: return Shift ^ CapsLock ? 'I' : 'i';
                    case CGLKey.J: return Shift ^ CapsLock ? 'J' : 'j';
                    case CGLKey.K: return Shift ^ CapsLock ? 'K' : 'k';
                    case CGLKey.L: return Shift ^ CapsLock ? 'L' : 'l';
                    case CGLKey.M: return Shift ^ CapsLock ? 'M' : 'm';
                    case CGLKey.N: return Shift ^ CapsLock ? 'N' : 'n';
                    case CGLKey.O: return Shift ^ CapsLock ? 'O' : 'o';
                    case CGLKey.P: return Shift ^ CapsLock ? 'P' : 'p';
                    case CGLKey.Q: return Shift ^ CapsLock ? 'Q' : 'q';
                    case CGLKey.R: return Shift ^ CapsLock ? 'R' : 'r';
                    case CGLKey.S: return Shift ^ CapsLock ? 'S' : 's';
                    case CGLKey.T: return Shift ^ CapsLock ? 'T' : 't';
                    case CGLKey.U: return Shift ^ CapsLock ? 'U' : 'u';
                    case CGLKey.V: return Shift ^ CapsLock ? 'V' : 'v';
                    case CGLKey.W: return Shift ^ CapsLock ? 'W' : 'w';
                    case CGLKey.X: return Shift ^ CapsLock ? 'X' : 'x';
                    case CGLKey.Y: return Shift ^ CapsLock ? 'Y' : 'y';
                    case CGLKey.Z: return Shift ^ CapsLock ? 'Z' : 'z';
                    case CGLKey.BackSlash: return Shift ? '|' : '\\';
                    case CGLKey.BracketLeft: return Shift ? '{' : '[';
                    case CGLKey.BracketRight: return Shift ? '}' : ']';
                    case CGLKey.Comma: return Shift ? '<' : ',';
                    case CGLKey.Grave: return Shift ? '~' : '`';
                    case CGLKey.Keypad0: return NumLock ? '0' : (char?)null;
                    case CGLKey.Keypad1: return NumLock ? '1' : (char?)null;
                    case CGLKey.Keypad2: return NumLock ? '1' : (char?)null;
                    case CGLKey.Keypad3: return NumLock ? '1' : (char?)null;
                    case CGLKey.Keypad4: return NumLock ? '1' : (char?)null;
                    case CGLKey.Keypad5: return NumLock ? '5' : (char?)null;
                    case CGLKey.Keypad6: return NumLock ? '6' : (char?)null;
                    case CGLKey.Keypad7: return NumLock ? '7' : (char?)null;
                    case CGLKey.Keypad8: return NumLock ? '8' : (char?)null;
                    case CGLKey.Keypad9: return NumLock ? '9' : (char?)null;
                    case CGLKey.KeypadPlus: return '+';
                    case CGLKey.KeypadDecimal: return NumLock ? '.' : (char?)null;
                    case CGLKey.KeypadDivide: return '/';
                    case CGLKey.KeypadMinus: return '-';
                    case CGLKey.KeypadMultiply: return '*';
                    case CGLKey.Number0: return Shift ? ')' : '0';
                    case CGLKey.Number1: return Shift ? '!' : '1';
                    case CGLKey.Number2: return Shift ? '@' : '2';
                    case CGLKey.Number3: return Shift ? '#' : '3';
                    case CGLKey.Number4: return Shift ? '$' : '4';
                    case CGLKey.Number5: return Shift ? '%' : '5';
                    case CGLKey.Number6: return Shift ? '^' : '6';
                    case CGLKey.Number7: return Shift ? '&' : '7';
                    case CGLKey.Number8: return Shift ? '*' : '8';
                    case CGLKey.Number9: return Shift ? '(' : '9';
                    case CGLKey.Period: return Shift ? '>' : '.';
                    case CGLKey.Plus: return Shift ? '+' : '=';
                    case CGLKey.Minus: return Shift ? '_' : '-';
                    case CGLKey.Quote: return Shift ? '"' : '\'';
                    case CGLKey.Semicolon: return Shift ? ':' : ';';
                    case CGLKey.Slash: return Shift ? '?' : '/';
                    case CGLKey.Space: return Shift ? ' ' : ' ';
                    default: return null;
                }
            }
        }

        public CGLKeyPress(CGLKey key, bool alt, bool shift, bool control, bool repeating, bool numLock, bool capsLock, bool scrollLock)
        {
            Key = key;
            Alt = alt;
            Shift = shift;
            Control = control;
            Repeating = repeating;
            NumLock = numLock;
            CapsLock = capsLock;
            ScrollLock = scrollLock;
        }

        public static bool operator ==(CGLKeyPress A, CGLKeyPress B)
        {
            if (object.ReferenceEquals(A, B)) return true;
            if (((object)A == null) || ((object)B == null))
            {
                return false;
            }
            else return (A.Key == B.Key && A.Alt == B.Alt && A.Shift == B.Shift && A.Control == B.Control && A.Repeating == B.Repeating);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (object.ReferenceEquals(this, obj)) { return true; }
            
            var other = obj as CGLKeyPress;
            return other != null && (Key == other.Key && Alt == other.Alt && Shift == other.Shift && Control == other.Control && Repeating == other.Repeating);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



        public static bool operator !=(CGLKeyPress A, CGLKeyPress B)
        {
            return !(A == B);
        }

    }
}
