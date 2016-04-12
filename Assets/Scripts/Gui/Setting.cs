using System;
using System.Linq;
using Assets.Scripts.Gui.Menu;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class Setting : MonoBehaviour
    {
        private AudioControl _audioControl;
        private Resolution[] _resolutions;
        private bool _valueChanged;
        public Slider BgmSlider;
        public YesNoWindow ChangeApplyWindow;
        public Dropdown GraphicQuality;
        public Dropdown Resolution;
        public Slider SoundEffectSlider;

        private void Awake()
        {
            _resolutions = Screen.resolutions;
            _audioControl = GameObject.FindGameObjectWithTag(Tag.Audio).GetComponent<AudioControl>();
            Resolution.AddOptions(_resolutions.Select(res => res.ToString().Split('@')[0]).ToList());
            GraphicQuality.AddOptions(QualitySettings.names.ToList());
            Initial();
            Load();
            BgmSlider.onValueChanged.AddListener(value =>
            {
                _audioControl.BackgroundMusic.volume = value;
                _valueChanged = true;
            });
            SoundEffectSlider.onValueChanged.AddListener(value =>
            {
                _audioControl.SoundEffect.volume = value;
                _valueChanged = true;
            });
            Resolution.onValueChanged.AddListener(
                index =>
                {
                    Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);
                    _valueChanged = true;
                });
            GraphicQuality.onValueChanged.AddListener(level =>
            {
                QualitySettings.SetQualityLevel(level, true);
                _valueChanged = true;
            });
            ChangeApplyWindow.YesButton.onClick.AddListener(() =>
            {
                Leave();
                ChangeApplyWindow.gameObject.SetActive(false);
            });
            ChangeApplyWindow.NoButton.onClick.AddListener(() => { ChangeApplyWindow.gameObject.SetActive(false); });
        }

        private void Initial()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefKey.BgmVolume))
                PlayerPrefs.SetFloat(PlayerPrefKey.BgmVolume, _audioControl.BackgroundMusic.volume);
            if (!PlayerPrefs.HasKey(PlayerPrefKey.SoundEffectVolume))
                PlayerPrefs.SetFloat(PlayerPrefKey.SoundEffectVolume, _audioControl.SoundEffect.volume);
            if (!PlayerPrefs.HasKey(PlayerPrefKey.ScreenWidth) || !PlayerPrefs.HasKey(PlayerPrefKey.ScreenHeight))
            {
                PlayerPrefs.SetInt(PlayerPrefKey.ScreenWidth, Screen.width);
                PlayerPrefs.SetInt(PlayerPrefKey.ScreenHeight, Screen.height);
            }
            var res = new Resolution
            {
                height = PlayerPrefs.GetInt(PlayerPrefKey.ScreenHeight),
                width = PlayerPrefs.GetInt(PlayerPrefKey.ScreenWidth),
                refreshRate = Screen.currentResolution.refreshRate
            };
            Resolution.value = Array.IndexOf(_resolutions, res);
            if (!PlayerPrefs.HasKey(PlayerPrefKey.GraphicQuality))
                PlayerPrefs.SetInt(PlayerPrefKey.GraphicQuality, QualitySettings.GetQualityLevel());
            GraphicQuality.value = QualitySettings.GetQualityLevel();
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(PlayerPrefKey.BgmVolume, _audioControl.BackgroundMusic.volume);
            PlayerPrefs.SetFloat(PlayerPrefKey.SoundEffectVolume, _audioControl.SoundEffect.volume);
            PlayerPrefs.SetInt(PlayerPrefKey.ScreenWidth, Screen.width);
            PlayerPrefs.SetInt(PlayerPrefKey.ScreenHeight, Screen.height);
            PlayerPrefs.SetInt(PlayerPrefKey.GraphicQuality, QualitySettings.GetQualityLevel());
            _valueChanged = false;
        }

        public void Load()
        {
            _audioControl.BackgroundMusic.volume = PlayerPrefs.GetFloat(PlayerPrefKey.BgmVolume);
            BgmSlider.value = _audioControl.BackgroundMusic.volume;
            _audioControl.SoundEffect.volume = PlayerPrefs.GetFloat(PlayerPrefKey.SoundEffectVolume);
            SoundEffectSlider.value = _audioControl.SoundEffect.volume;
            Screen.SetResolution(PlayerPrefs.GetInt(PlayerPrefKey.ScreenWidth),
                PlayerPrefs.GetInt(PlayerPrefKey.ScreenHeight), Screen.fullScreen);
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(PlayerPrefKey.GraphicQuality), true);
        }

        public void CheckApplyValue()
        {
            if (!_valueChanged)
                Leave();
            else
                ChangeApplyWindow.gameObject.SetActive(true);
        }

        private void Leave()
        {
            Load();
            GetComponent<ChangeMenu>().Change();
        }
    }
}