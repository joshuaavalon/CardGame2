using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class BgmSlider : MonoBehaviour
    {
        private AudioSource _bgm;
        private Slider _slider;

        public void Awake()
        {
            _slider = GetComponent<Slider>();
            var control = GameObject.FindGameObjectWithTag(Tag.Audio).GetComponent<AudioControl>();
            _bgm = control.BackgroundMusic;
            if (!PlayerPrefs.HasKey(PlayerPrefKey.BgmVolume))
                Save();
            Load();
            _slider.onValueChanged.AddListener(value => { _bgm.volume = value; });
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(PlayerPrefKey.BgmVolume, _bgm.volume);
        }

        public void Load()
        {
            _bgm.volume = PlayerPrefs.GetFloat(PlayerPrefKey.BgmVolume);
            _slider.value = _bgm.volume;
        }
    }
}