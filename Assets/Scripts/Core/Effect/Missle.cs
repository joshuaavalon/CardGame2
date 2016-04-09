using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Core.Effect
{
    public class Missle : BaseEffect
    {
        public int MissleDamage;

        public override void OnEnter()
        {
            var game = Parent.Parent.Game;
            var opponent = game.GetPlayer(Parent.Parent.Type.Opposite());
            if (!opponent.Battlefield.Any()) return;
            var cardList = (IList<Card>)opponent.Battlefield;
            foreach (var card in cardList.ToList())
            {
                card.TakeDamage(MissleDamage);
            }
        }
    }
}