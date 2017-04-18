using System;
using System.Text;
using MistwoodTales.Game.Client.RLNet.Entities;
using MistwoodTales.Game.Client.RLNet.Scheduling;
using RogueSharp;
using RogueSharp.DiceNotation;

namespace MistwoodTales.Game.Client.RLNet.Systems
{
    public class CommandSystem
    {
        public static bool RedrawNeeded { get; set; }

        // Return value is true if the player was able to move
        // false when the player couldn't move, such as trying to move into a wall
        //public bool MovePlayer(Direction direction)
        //{
        //    int x = Game.Player.X;
        //    int y = Game.Player.Y;

        //    switch (direction)
        //    {
        //        case Direction.Up:
        //            {
        //                y = Game.Player.Y - 1;
        //                break;
        //            }
        //        case Direction.Down:
        //            {
        //                y = Game.Player.Y + 1;
        //                break;
        //            }
        //        case Direction.Left:
        //            {
        //                x = Game.Player.X - 1;
        //                break;
        //            }
        //        case Direction.Right:
        //            {
        //                x = Game.Player.X + 1;
        //                break;
        //            }
        //        default:
        //            {
        //                return false;
        //            }
        //    }

        //    if (Game.CurrentMap.SetActorPosition(Game.Player, x, y))
        //    {
        //        RedrawNeeded = true;
        //        return true;
        //    }
        //    Monster monster = Game.CurrentMap.GetMonsterAt(x, y);

        //    if (monster != null)
        //    {
        //        Attack(Game.Player, monster);
        //        RedrawNeeded = true;
        //        return true;
        //    }

        //    return false;
        //}

        public void Attack(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            int hits = ResolveAttack(attacker, defender, attackMessage);

            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            Game.MessageLog.Add(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                Game.MessageLog.Add(defenseMessage.ToString());
            }

            int damage = hits - blocks;

            ResolveDamage(defender, damage);
        }

        // The attacker rolls based on his stats to see if he gets any hits
        private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;

            attackMessage.AppendFormat("{0} attacks {1} and rolls: ", attacker.Name, defender.Name);

            // Roll a number of 100-sided dice equal to the Attack value of the attacking actor
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 100);
            DiceResult attackResult = attackDice.Roll();

            // Look at the face value of each single die that was rolled
            foreach (TermResult termResult in attackResult.Results)
            {
                attackMessage.Append(termResult.Value + ", ");
                // Compare the value to 100 minus the attack chance and add a hit if it's greater
                if (termResult.Value >= 100 - attacker.AttackChance)
                {
                    hits++;
                }
            }

            RedrawNeeded = true;
            return hits;
        }

        // The defender rolls based on his stats to see if he blocks any of the hits from the attacker
        private static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;

            if (hits > 0)
            {
                attackMessage.AppendFormat("scoring {0} hits.", hits);
                defenseMessage.AppendFormat("  {0} defends and rolls: ", defender.Name);

                // Roll a number of 100-sided dice equal to the Defense value of the defendering actor
                DiceExpression defenseDice = new DiceExpression().Dice(defender.Defense, 100);
                DiceResult defenseRoll = defenseDice.Roll();

                // Look at the face value of each single die that was rolled
                foreach (TermResult termResult in defenseRoll.Results)
                {
                    defenseMessage.Append(termResult.Value + ", ");
                    // Compare the value to 100 minus the defense chance and add a block if it's greater
                    if (termResult.Value >= 100 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.AppendFormat("resulting in {0} blocks.", blocks);
            }
            else
            {
                attackMessage.Append("and misses completely.");
            }
            RedrawNeeded = true;
            return blocks;
        }

        // Apply any damage that wasn't blocked to the defender
        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;

                Game.MessageLog.Add($"  {defender.Name} was hit for {damage} damage");

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                Game.MessageLog.Add($"  {defender.Name} blocked all damage");
            }
            RedrawNeeded = true;
        }

        // Remove the defender from the map and add some messages upon death.
        private static void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                Game.MessageLog.Add($"  {defender.Name} was killed, GAME OVER MAN!");
            }
            else if (defender is Monster)
            {
                Game.CurrentMap.RemoveMonster((Monster)defender);

                Game.MessageLog.Add($"  {defender.Name} died and dropped {defender.Gold} gold");
            }
            RedrawNeeded = true;
        }

        public void ActivateActors()
        {
            Game.SchedulingSystem.RunActions();

        }

        public void MonsterMoveOrAttack(Monster monster, ICell cell)
        {
            var player = Game.Player;
            if (cell.X == player.X && cell.Y == player.Y // targeting player
                && PlayerIsClose(monster.X, monster.Y))
            {
                Attack(monster, Game.Player);
            }
            else if (Game.CurrentMap.SetActorPosition(monster, cell.X, cell.Y))
            {
                RedrawNeeded = true;
            }
        }

        private bool PlayerIsClose(int x, int y)
        {
            var player = Game.Player;
            int dx = Math.Abs(player.X - x);
            int dy = Math.Abs(player.Y - y);
            return dx <= 1 && dy <= 1;
        }

        public void SetPlayerPath(int x, int y)
        {
            var map = Game.CurrentMap;
            if (x < 0
                || y < 0
                || x > map.Width - 1
                || y > map.Height - 1)
                return;
            var player = Game.Player;

            if (x == player.X && y == player.Y) 
                return;
            var destCell = map.GetCell(x, y);
            if (!destCell.IsInFov
                || !destCell.IsWalkable)
                return;
            var playerCell = map.GetCell(player.X, player.Y);
            try
            {
                //map.SetIsWalkable(playerCell.X, playerCell.Y, true);
                var path = new PathFinder(map).ShortestPath(playerCell, destCell);
                player.SetDestinationPath(path);
            }
            catch (PathNotFoundException) { }
            finally
            {
                //map.SetIsWalkable(playerCell.X, playerCell.Y, false);
            }
        }

        public void Act()
        {
            ActivateActors();
        }
    }
}
