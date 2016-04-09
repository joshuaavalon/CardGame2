using System.Linq;
using Assets.Scripts.Core.Statistics;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Core.Effect
{
    public class Charge : BaseEffect
    {
        public override void OnEnter()
        {
            Parent.FirstTurnPlay = false;
        }
    }
}