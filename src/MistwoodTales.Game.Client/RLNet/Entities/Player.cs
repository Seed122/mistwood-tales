using MistwoodTales.Game.Client.RLNet.Base;
using MistwoodTales.Game.Client.RLNet.Scheduling;
using ConsoleGL;
using RogueSharp;

namespace MistwoodTales.Game.Client.RLNet.Entities
{
    public class Player: Actor
    {

        public Player()
        {
            LightRadius = 15;
            Name = "Gatmeat";
            Color = Colors.Player;
            Symbol = '@';
            X = 20;
            Y = 20;
            Speed = 6;
            Attack = 8;
            AttackChance = 20;
            Defense = 3;
            DefenseChance = 20;
            MaxHealth = 100;
            Health = MaxHealth;
            Gold = 50;
            _pathMoveBehavior = new PathMoveBehavior(this);
            Behaviors.Add(_pathMoveBehavior);
        }

        private readonly PathMoveBehavior _pathMoveBehavior;

        public void DrawStats(CGLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Name:    {Name}", Colors.Text);
            statConsole.Print(1, 3, $"Health:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 5, $"Attack:  {Attack} ({AttackChance}%)", Colors.Text);
            statConsole.Print(1, 7, $"Defense: {Defense} ({DefenseChance}%)", Colors.Text);
            statConsole.Print(1, 9, $"Gold:    {Gold}", Colors.Gold);
        }

        public void SetDestinationPath(Path path)
        {
            _pathMoveBehavior.DestinationPath = path;
        }
    }
}
