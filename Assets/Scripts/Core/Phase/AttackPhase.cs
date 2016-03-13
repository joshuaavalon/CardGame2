using System.Linq;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Core.Phase
{
    public class AttackPhase : BasePhase
    {
        public AttackPhase(Game game, PlayerType parent) : base(game, parent)
        {
        }

        protected override BasePhase NextPhase()
        {
            return new DefencePhase(Game, Parent.Opposite());
        }

        public override string GetName()
        {
            return "Attack Phase";
        }

        protected override void Execute()
        {
            if(Parent!=PlayerType.Player) return;
            var player = Game.GetPlayer(PlayerType.Player);
            var selectable = player.GetAttackUnit().Select(card => card.Id).ToArray();
            Game.GuiMediator.EnableSelection(attacker => { Game.CreateBattle(PlayerType.Player, attacker); }
                , selectable, true, false);
        }
    }
}