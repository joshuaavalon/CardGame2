using System;
using System.Linq;
using Assets.Scripts.Audio;
using Assets.Scripts.Configuration;
using Assets.Scripts.Core;
using Assets.Scripts.Gui.Menu;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class Setting : MonoBehaviour
    {
        private AudioControl _audioControl;
        private GameConfiguration _config;
        private Resolution[] _resolutions;
        private bool _valueChanged;
        public Slider BgmSlider;
        public YesNoWindow ChangeApplyWindow;
        public Dropdown GraphicQuality;
        public Dropdown Resolution;
        public Slider SoundEffectSlider;

        private void Awake()
        {
            _config = GameConfiguration.Get();
            _resolutions = Screen.resolutions;
            _audioControl = GameObject.FindGameObjectWithTag(Tag.Audio).GetComponent<AudioControl>();
            Resolution.AddOptions(_resolutions.Select(res => res.ToString().Split('@')[0]).ToList());
            GraphicQuality.AddOptions(QualitySettings.names.ToList());
            Load();
            BgmSlider.onValueChanged.AddListener(value =>
            {
                _audioControl.BackgroundMusic.volume = value;
                _valueChanged = true;
            });
            SoundEffectSlider.onValueChanged.AddListener(value =>
            {
                _audioControl.PlayAudioClip(AudioClipType.Greetings, value);
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

        private void OnEnable()
        {
            Load();
        }

        public void Save()
        {
            _config.BackgroundMusicVolume = BgmSlider.value;
            _config.SoundEffectVolume = SoundEffectSlider.value;
            _config.GraphicQualityLevel = GraphicQuality.value;
            _config.Resolution = new Resolution
            {
                height = Screen.height,
                width = Screen.width,
                refreshRate = Screen.currentResolution.refreshRate
            };
            _valueChanged = false;
        }

        public void Load()
        {
            Resolution.value = Array.IndexOf(_resolutions, _config.Resolution);
            GraphicQuality.value = _config.GraphicQualityLevel;
            BgmSlider.value = _config.BackgroundMusicVolume;
            SoundEffectSlider.value = _config.SoundEffectVolume;
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
            GetComponent<ChangeMenu>().Change();
            _audioControl.ResetVolume();
        }
    }
}