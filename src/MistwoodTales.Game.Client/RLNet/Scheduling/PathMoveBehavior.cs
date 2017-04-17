using MistwoodTales.Game.Client.RLNet.Entities;
using MistwoodTales.Game.Client.RLNet.Systems;
using RogueSharp;

namespace MistwoodTales.Game.Client.RLNet.Scheduling
{
    class PathMoveBehavior: IBehavior
    {
        private readonly Actor _actor;

        public PathMoveBehavior(Actor actor)
        {
            _actor = actor;
        }

        public Path DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                _destinationPath = value;
                _isMoving = false;
            }
        }

        private bool _isMoving = false;
        private Path _destinationPath;

        public bool Act(CommandSystem commandSystem)
        {
            if (DestinationPath == null)
                return false;
            if (DestinationPath.End.X == _actor.X && DestinationPath.End.Y == _actor.Y)
            {
                DestinationPath = null;
                return false;
            }
            else
            {
                var cell = DestinationPath.CurrentStep.Equals(DestinationPath.Start) && !_isMoving
                    ? DestinationPath.Start
                    : DestinationPath.StepForward();
                _isMoving = true;
                bool moveResult = Game.DungeonMap.SetActorPosition(_actor, cell.X, cell.Y);
                if (!moveResult)
                {
                    DestinationPath = null;
                }
                return moveResult;
            }
        }
    }
}
