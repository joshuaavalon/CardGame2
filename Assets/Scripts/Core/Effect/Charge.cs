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