namespace Assets.Scripts.Core.Effect
{
    public class Clone : BaseEffect
    {
        public string Hallucination;
        public override int TakeDamageMod(int damage)
        {
            var player = Parent.Parent;
            if (damage <= 0 || player.Type == PlayerType.Opponent) return damage;
            var game = player.Game;
            var id = game.GetCardId(player.Type);
            game.CreateCard(Hallucination,id, player.Type,ZoneType.BattleField);
            return damage;
        }
    }
}