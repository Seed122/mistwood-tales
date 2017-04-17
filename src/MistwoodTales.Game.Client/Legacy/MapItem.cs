namespace MistwoodTales.Game.Client.Legacy
{
    //public struct MapItemColor
    //{
    //    public MapItemColor(ConsoleForeground foreground, ConsoleBackground background)
    //    {
    //        Background = background;
    //        Foreground = foreground;
    //    }

    //    public Microsoft.GotDotNet.ConsoleBackground Background;
    //    public Microsoft.GotDotNet.ConsoleForeground Foreground;

    //    public override bool Equals(object obj)
    //    {
    //        return base.Equals(obj);
    //    }

    //    public bool Equals(MapItemColor other)
    //    {
    //        return Background == other.Background && Foreground == other.Foreground;
    //    }

    //    public override int GetHashCode()
    //    {
    //        unchecked
    //        {
    //            return ((int) Background * 397) ^ (int) Foreground;
    //        }
    //    }

    //    public static bool operator ==(MapItemColor left, MapItemColor right)
    //    {
    //        return left.Equals(right);
    //    }

    //    public static bool operator !=(MapItemColor left, MapItemColor right)
    //    {
    //        return !left.Equals(right);
    //    }
    //}

    public struct MapItem
    {
        public bool CanWalk;
        public char Symbol;
        //public MapItemColor Color;
        
    }
}
