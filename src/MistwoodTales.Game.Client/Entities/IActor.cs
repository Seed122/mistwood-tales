using System.Collections.Generic;
using MistwoodTales.Game.Client.Scheduling;

namespace MistwoodTales.Game.Client.Entities
{
    public interface IActor
    {
        int Attack { get; set; }
        int AttackChance { get; set; }
        int LightRadius { get; set; }
        int Defense { get; set; }
        int DefenseChance { get; set; }
        int Gold { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        string Name { get; set; }
        int Speed { get; set; }

        ICollection<IBehavior> Behaviors { get; }
    }
}
