namespace Assets.Scripts.Core.Message
{
    public class TurnStartMessage : GameMessage
    {
        public readonly PlayerType Parent;

        public TurnStartMessage(PlayerType parent)
        {
            Parent = parent;
        }
    }
}