using System.Linq;
using Assets.Scripts.Core.Statistics;

namespace Assets.Scripts.Core.Effect
{
    //When the battlefield has the same type of spacecraft, gain +1/+1.
    public class AllyByTypeBoost : BaseEffect
    {
        public UnitType Tags;
        public override void OnEnter()
        {
            var game = Parent.Parent.Game;
            var player = game.GetPlayer(Parent.Parent.Type);
            var cards = player.Battlefield;
            var counter = 0;
            foreach (var card in cards)
            {
                if (card == Parent) break;
                counter += card.Tags.Count(unitType => unitType.Equals(Tags));
            }

            Parent.SetStats(CardStatsType.Atk, Parent.GetStats(CardStatsType.Atk) + counter);
            Parent.SetStats(CardStatsType.Hp, Parent.GetStats(CardStatsType.Hp) + counter);
        }
    }
}