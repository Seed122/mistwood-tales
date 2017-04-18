using MistwoodTales.Game.Client.Systems;

namespace MistwoodTales.Game.Client.Scheduling
{
    public interface IBehavior
    {
        bool Act(CommandSystem commandSystem);

    }
}
