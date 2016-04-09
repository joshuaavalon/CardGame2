using Assets.Scripts.Core.Statistics;

namespace Assets.Scripts.Core.Effect
{
    public class Repair : BaseEffect
    {
        public int Repairvalue;

        public override void OnEnter()
        {
            var player = Parent.Parent;
            if (player.GetStats(PlayerStatsType.Hp) >= player.GetStats(PlayerStatsType.MaxHp)) return;
            var newHp = player.GetStats(PlayerStatsType.Hp) + Repairvalue;
            player.SetStats(PlayerStatsType.Hp, (newHp < player.GetStats(PlayerStatsType.MaxHp)) ? newHp : player.GetStats(PlayerStatsType.MaxHp));
        }
    }
}