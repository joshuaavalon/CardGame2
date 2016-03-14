using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class Tutorial : MonoBehaviour
    {
        public Sprite[] Images;
        public Image MainImage;
        public GameObject RadioButtonPrefab;

        [HideInInspector]
        public int Current { get; private set; }

        public void Toggle(int number)
        {
            MainImage.sprite = Images[number];
            Current = number;
        }

        private void Start()
        {
            for (var i = 0; i < Images.Length; i++)
            {
                var radio = Instantiate(RadioButtonPrefab);
                radio.transform.SetParent(transform, false);
                var toggle = radio.GetComponent<Toggle>();
                toggle.group = GetComponent<ToggleGroup>();
                var j = i;
                toggle.isOn = i == 0;
                toggle.onValueChanged.AddListener(value => { if (value) Toggle(j); });
            }
            if (Images.Length > 0)
                Toggle(0);
        }
    }
}