using Assets.Scripts.Core.Message;
using Assets.Scripts.Core.Statistics;
using Assets.Scripts.Infrastructure.EventAggregator;

namespace Assets.Scripts.Core.Effect
{
    //At each Main Phase, Gain +1/+1.
    public class Evolution : BaseEffect, IHandle<PhaseStartMessage>
    {
        public int AttackBonus;
        public int HpBonus;

        public override void OnEnter()
        {
            Parent.Parent.Game.Subscribe(this);
        }
        
        public void Handle(PhaseStartMessage message)
        {
            var phase = message.Phase;
            if (phase.GetParent() != Parent.Parent.Type || phase.GetName() != "Main Phase") return;
            Parent.SetStats(CardStatsType.Atk, Parent.GetStats(CardStatsType.Atk) + AttackBonus);
            Parent.SetStats(CardStatsType.Hp, Parent.GetStats(CardStatsType.Hp) + HpBonus);
        }
    }
}