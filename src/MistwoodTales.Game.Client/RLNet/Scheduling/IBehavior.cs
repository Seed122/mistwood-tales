using MistwoodTales.Game.Client.RLNet.Systems;

namespace MistwoodTales.Game.Client.RLNet.Scheduling
{
    public interface IBehavior
    {
        bool Act(CommandSystem commandSystem);

    }
}
