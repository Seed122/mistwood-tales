namespace MistwoodTales.Game.Client.Legacy
{
    public class MoveableEntity: Entity
    {
        public Direction FaceDirection { get; set; }

        public virtual void Move(Direction direction)
        {
            var map = World.Instance.Map;
            int newX = 0, newY = 0;
            switch (direction)
            {
                case Direction.Left:
                    newX = Point.X - 1;
                    newY = Point.Y;
                    break;
                case Direction.Right:
                    newX = Point.X + 1;
                    newY = Point.Y;
                    break;
                case Direction.Up:
                    newX = Point.X;
                    newY = Point.Y - 1;
                    break;
                case Direction.Down:
                    newX = Point.X;
                    newY = Point.Y + 1;
                    break;
                default:
                    break;
            }
            if (!map.CheckIfCoordsAreAvailable(newX, newY))
                return;
            Point.X = newX;
            Point.Y = newY;
            FaceDirection = direction;
        }

    }
}
