namespace Assets.Scripts.Core.Effect
{
    //Used to draw cards.
    public class DrawCard : BaseEffect
    {
        public int DrawCardNumber ;
        public override void OnEnter()
        {
            var player = Parent.Parent.Type;
            if (player != PlayerType.Player) return;
            for (var i = 0; i < DrawCardNumber; i++)
            {
                Parent.Parent.Game.DrawCard(player);
            }
        }
    }
}