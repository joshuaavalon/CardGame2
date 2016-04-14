using System.Linq;
using Assets.Scripts.Core.Statistics;

namespace Assets.Scripts.Core.Effect
{
    public class FriendlyFire : BaseEffect
    {
        public int AttackBonus;
        public int HpBonus;

        public override void OnEnter()
        {
            var cards = Parent.Parent.Battlefield;
            foreach (var card in cards.ToList())
            {
                if (card == Parent) break;
                card.SetStats(CardStatsType.Atk, card.GetStats(CardStatsType.Atk) + AttackBonus);
                card.SetStats(CardStatsType.Hp, card.GetStats(CardStatsType.Hp) + HpBonus);
            }
        }
    }
}