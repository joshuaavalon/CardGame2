using Assets.Scripts.Core.Statistics;
// ReSharper disable MergeConditionalExpression

namespace Assets.Scripts.Core.Effect
{
    public class Explosion : BaseEffect
    {
        public override int GetDamageMod(IDamagable target, int damage)
        {
            var card = target as Card;
            return card == null ? damage : card.GetStats(CardStatsType.Hp);
        }
    }
}