using Assets.Scripts.Core.Phase;

namespace Assets.Scripts.Core.Message
{
    public class PhaseEndMessage : GameMessage
    {
        public readonly IPhase Phase;

        public PhaseEndMessage(IPhase phase)
        {
            Phase = phase;
        }
    }
}