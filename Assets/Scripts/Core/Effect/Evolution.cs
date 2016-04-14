using Assets.Scripts.Core.Statistics;

namespace Assets.Scripts.Core.Effect
{
    //At each Main Phase, Gain +1/+1.
    public class Evolution : BaseEffect
    {
        public int AttackBonus;
        public int HpBonus;
        
        public override void OnAttack()
        {
            Parent.SetStats(CardStatsType.Atk, Parent.GetStats(CardStatsType.Atk) + AttackBonus);
            Parent.SetStats(CardStatsType.Hp, Parent.GetStats(CardStatsType.Hp) + HpBonus);
        }
    }
}