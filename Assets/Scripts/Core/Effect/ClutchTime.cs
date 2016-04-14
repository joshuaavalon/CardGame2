using Assets.Scripts.Core.Statistics;

namespace Assets.Scripts.Core.Effect
{
    public class ClutchTime : BaseEffect
    {
        public int ClutchValue;
        public int AttackBonus;
        public int HpBonus;

        public override void OnEnter()
        {
            if (Parent.Parent.GetStats(PlayerStatsType.Hp) > ClutchValue) return;
            Parent.SetStats(CardStatsType.Atk, Parent.GetStats(CardStatsType.Atk) + AttackBonus);
            Parent.SetStats(CardStatsType.Hp, Parent.GetStats(CardStatsType.Hp) + HpBonus);
        }

    }
}