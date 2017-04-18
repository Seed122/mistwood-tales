using System.Linq;
using MistwoodTales.Game.Client.Entities;
using MistwoodTales.Game.Client.Systems;
using MistwoodTales.Game.Client.World;
using RogueSharp;

namespace MistwoodTales.Game.Client.Scheduling
{
    public class StandardMoveAndAttackBehavior : IBehavior
    {
        public StandardMoveAndAttackBehavior(Monster monster)
        {
            _monster = monster;
        }

        private Path _agroPath;
        private int _agroPathStepIndex;
        private Path _returnPath;
        private readonly Monster _monster;

        public bool Act(CommandSystem commandSystem)
        {
            var map = Game.CurrentMap;
            Player player = Game.Player;
            FieldOfView monsterFov = new FieldOfView(map);

            // If the monster has not been alerted, compute a field-of-view 
            // Use the monster's LightRadius value for the distance in the FoV check
            // If the player is in the monster's FoV then alert it
            // Add a message to the MessageLog regarding this alerted status
            if (!_monster.TurnsAlerted.HasValue)
            {
                monsterFov.ComputeFov(_monster.X, _monster.Y, _monster.LightRadius, true);
                if (monsterFov.IsInFov(player.X, player.Y))
                {
                    // Game.MessageLog.Add($"{_monster.Name} is eager to fight {player.Name}");
                    _monster.TurnsAlerted = 1;
                }
            }

            if (_monster.TurnsAlerted.HasValue)
            {
                InitAgroPath(map, _monster, player);
                try
                {
                    // TODO: This should be path.StepForward() but there is a bug in RogueSharp V3
                    // The bug is that a Path returned from a PathFinder does not include the source Cell
                    
                    commandSystem.MonsterMoveOrAttack(_monster, _agroPath.Steps.First());
                }
                catch (NoMoreStepsException)
                {
                    // Game.MessageLog.Add($"{_monster.Name} growls in frustration");
                }

                _monster.TurnsAlerted++;

                // Lose alerted status every 15 turns. 
                // As long as the player is still in FoV the monster will stay alert
                // Otherwise the monster will quit chasing the player.
                if (_monster.TurnsAlerted > 15)
                {
                    _monster.TurnsAlerted = null;
                }
            }
            return true;
        }

        private void InitAgroPath(MistwoodMap dungeonMap, Actor monster, Actor player)
        {
            try
            {
                // Before we find a path, make sure to make the monster and player Cells walkable
                //dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
                //dungeonMap.SetIsWalkable(player.X, player.Y, true);
                var monsterCell = dungeonMap.GetCell(monster.X, monster.Y);
                var playerCell = dungeonMap.GetCell(player.X, player.Y);
                if (monsterCell.Equals(playerCell))
                    return;
                var pf = new PathFinder(dungeonMap);
                _agroPath = pf.ShortestPath(monsterCell, playerCell);
            }
            catch (PathNotFoundException)
            {
                // The monster can see the player, but cannot find a path to him
                // This could be due to other monsters blocking the way
                // Add a message to the message log that the monster is waiting
                // Game.MessageLog.Add($"{monster.Name} waits for a turn");
            }
            finally
            {

                // Don't forget to set the walkable status back to false
                //dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
                //dungeonMap.SetIsWalkable(player.X, player.Y, false);
            }

        }
    }
}
