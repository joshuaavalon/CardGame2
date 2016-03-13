using System.Linq;

namespace Assets.Scripts.Core.Effect
{
    public class DamageByType : BaseEffect
    {
        public int NormalDamage;
        public int SpecialDamage;
        public UnitType TargetType;

        public override void OnEnter(Card target)
        {
            target.TakeDamage(target.Tags.Contains(TargetType) ? SpecialDamage : NormalDamage);
        }
    }
}