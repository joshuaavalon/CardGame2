namespace Assets.Scripts.Core.Message
{
    public class CardPlayMessage : GameMessage
    {
        public readonly Card Card;

        public CardPlayMessage(Card card)
        {
            Card = card;
        }
    }
}