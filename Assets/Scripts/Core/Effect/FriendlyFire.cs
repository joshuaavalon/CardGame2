using System.Linq;
using Assets.Scripts.Core.Statistics;

namespace Assets.Scripts.Core.Effect
{
    //When the battlefield has the same type of spacecraft, gain +1/+1.
    public class FriendlyFire : BaseEffect
    {
        public int AttackBonus;
        public int HpBonus;

        public override void OnEnter()
        {
            var game = Parent.Parent.Game;
            var player = game.GetPlayer(Parent.Parent.Type);
            var cards = player.Battlefield;
            foreach (var card in cards)
            {
                if (card == Parent) break;
                card.SetStats(CardStatsType.Atk, Parent.GetStats(CardStatsType.Atk) + AttackBonus);
                card.SetStats(CardStatsType.Hp, Parent.GetStats(CardStatsType.Hp) + HpBonus);
            }
        }
    }
}