namespace Assets.Scripts.Core.Effect
{
    public class CreateAlly : BaseEffect
    {
        public string Unix;

        public override void OnEnter()
        {
            var player = Parent.Parent;
            if (player.Type == PlayerType.Opponent) return;
            var game = player.Game;
            var id = game.GetCardId(player.Type);
            game.CreateCard(Unix, id, player.Type, ZoneType.BattleField);
        }

    }
}