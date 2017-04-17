namespace MistwoodTales.Game.Client.Legacy
{
    public class Player: MoveableEntity
    {
        public Player()
        {
            Direction = Direction.Right;
            Symbol = '%';
            Point = new Point(120, 20);
        }

        public int HP { get; set; }
        public int MaxHP { get; set; }

        public Direction Direction { get; set; }
        public string Name { get; set; }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0)
            {
                HP = 0;
            }
        }
    }
}
