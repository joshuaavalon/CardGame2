using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Core.Phase
{
    public class SecondMainPhase : MainPhase
    {
        public SecondMainPhase(Game game, PlayerType parent) : base(game, parent)
        {
        }

        protected override void Execute()
        {
            Game.ResolveBattle();
            if(Parent==PlayerType.Player) return;
            var player = Game.GetPlayer(PlayerType.Player);
            foreach (var id in player.GetCardOnBattleField())
            {
                Game.GuiMediator.SetDraggable(id,false);
            }
        }

        protected override BasePhase NextPhase()
        {
            return new MainPhase(Game, Parent.Opposite());
        }

        public override string GetName()
        {
            return "Second Main Phase";
        }
    }
}