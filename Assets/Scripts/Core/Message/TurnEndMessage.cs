namespace Assets.Scripts.Core.Message
{
    public class TurnEndMessage : GameMessage
    {
        public readonly PlayerType Parent;

        public TurnEndMessage(PlayerType parent)
        {
            Parent = parent;
        }
    }
}
