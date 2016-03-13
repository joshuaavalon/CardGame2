using Assets.Scripts.Core.Phase;

namespace Assets.Scripts.Core.Message
{
    public class PhaseStartMessage : GameMessage
    {
        public readonly IPhase Phase;

        public PhaseStartMessage(IPhase phase)
        {
            Phase = phase;
        }
    }
}