using Assets.Scripts.Core.Statistics;
using UnityEngine;

namespace Assets.Scripts.Gui.Controller
{
    public class StatisticsController : MonoBehaviour
    {
        public Bar CrystalBar;
        public Bar DeuteriumBar;
        public Bar HpBar;
        public Bar MetalBar;

        /// <summary>
        ///     Set the text of the text box.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public void SetText(PlayerStatsType type, int current, int max)
        {
            switch (type)
            {
                case PlayerStatsType.Hp:
                    HpBar.SetValue(current, max);
                    break;
                case PlayerStatsType.Metal:
                    MetalBar.SetValue(current, max);
                    break;
                case PlayerStatsType.Crystal:
                    CrystalBar.SetValue(current, max);
                    break;
                case PlayerStatsType.Deuterium:
                    DeuteriumBar.SetValue(current, max);
                    break;
            }
        }
    }
}