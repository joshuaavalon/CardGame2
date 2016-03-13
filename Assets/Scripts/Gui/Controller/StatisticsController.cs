using Assets.Scripts.Core.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Controller
{
    public class StatisticsController : MonoBehaviour
    {
        public Text CrystalText;
        public Text DeuteriumText;
        public Text HpText;
        public Text MetalText;
        public PlayerController Parent;

        /// <summary>
        ///     Set the text of the text box.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public void SetText(PlayerStatsType type, string text)
        {
            // TODO: Set text
            switch (type)
            {
                case PlayerStatsType.Hp:
                    HpText.text = text;
                    break;
                case PlayerStatsType.Metal:
                    MetalText.text = text;
                    break;
                case PlayerStatsType.Crystal:
                    CrystalText.text = text;
                    break;
                case PlayerStatsType.Deuterium:
                    DeuteriumText.text = text;
                    break;
            }
        }
    }
}