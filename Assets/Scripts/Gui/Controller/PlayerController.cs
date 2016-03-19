using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Gui.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject Battlefield;
        public GameObject Hand;
        public StatisticsController Stats;

        /// <summary>
        ///     Move card to hand.
        /// </summary>
        /// <param name="card">Card to move.</param>
        public void MoveToHand(GameObject card)
        {
            card.MoveToParent(Hand);
        }

        /// <summary>
        ///     Move card to Battlefield.
        /// </summary>
        /// <param name="card">Card to move.</param>
        public void MoveToBattlefield(GameObject card)
        {
            card.MoveToParent(Battlefield);
        }
    }
}