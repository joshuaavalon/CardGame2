using System.Linq;
using Assets.Scripts.Core.Statistics;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Core.Effect
{
    public class EnterEmptyBattleFieldBouns : BaseEffect
    {
        public int AttackBonus;
        public int HpBonus;

        public override void OnEnter()
        {
            var game = Parent.Parent.Game;
            var opponent = game.GetPlayer(Parent.Parent.Type.Opposite());
            if (opponent.Battlefield.Any()) return;
            Parent.SetStats(CardStatsType.Atk, Parent.GetStats(CardStatsType.Atk) + AttackBonus);
            Parent.SetStats(CardStatsType.Hp, Parent.GetStats(CardStatsType.Hp) + HpBonus);
        }
    }
}