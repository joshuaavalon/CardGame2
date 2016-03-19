using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class Bar : MonoBehaviour
    {
        private int _current;
        private int _max;
        public Image Fill;
        public bool ShowMax;
        public Text Text;

        public void SetValue(int current, int max)
        {
            _current = current;
            _max = max;
            UpdateUi();
        }

        private void UpdateUi()
        {
            Text.text = _current + (ShowMax ? " / " + _max : "");
            if (Fill != null)
                Fill.fillAmount = _current/(float) _max;
        }
    }
}