using System.Linq;
using Assets.Scripts.Gui.Event;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Core.Phase
{
    public class DefencePhase : BasePhase
    {
        public DefencePhase(Game game, PlayerType parent) : base(game, parent)
        {
        }

        protected override void Execute()
        {
            if (Parent != PlayerType.Player) return;
            var player = Game.GetPlayer(PlayerType.Player);
            foreach (var id in player.GetDefenceUnit().Select(card => card.Id))
                Game.GuiMediator.SetDraggable(id, true);
        }

        protected override BasePhase NextPhase()
        {
            return new SecondMainPhase(Game, Parent.Opposite());
        }

        public override string GetName()
        {
            return "Defence Phase";
        }

        public override void Handle(CardDragToCardEventArgs args)
        {
            if (Game.GetCardById(args.Target).Zone != ZoneType.BattleField) return;
            if (Game.GetCardById(args.Destination).Zone != ZoneType.BattleField) return;
            Game.AddBattle(args.Target, args.Destination);
        }
    }
}