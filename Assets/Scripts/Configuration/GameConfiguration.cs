using System;
using Assets.Scripts.Metadata;
using UnityEngine;

// ReSharper disable ConvertIfStatementToNullCoalescingExpression

namespace Assets.Scripts.Configuration
{
    public class GameConfiguration
    {
        public const float DefaultBackgroundMusicVolume = 0.5f;
        public const float DefaultSoundEffectVolume = 1f;
        private static GameConfiguration _instance;
        private float _backgroundMusicVolume;
        private int _graphicQualityLevel;
        private Resolution _resolution;
        private float _soundEffectVolume;

        public Action<float> OnBackgroundMusicVolumeChange =
            volume => { PlayerPrefs.SetFloat(PlayerPrefKey.BgmVolume, volume); };

        public Action<int> OnGraphicQualityLevelChange = level =>
        {
            QualitySettings.SetQualityLevel(level, true);
            PlayerPrefs.SetInt(PlayerPrefKey.GraphicQuality, level);
        };

        public Action<Resolution> OnResolutionChange = resolution =>
        {
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt(PlayerPrefKey.ScreenWidth, resolution.width);
            PlayerPrefs.SetInt(PlayerPrefKey.ScreenHeight, resolution.height);
        };

        public Action<float> OnSoundEffectVolumeChange =
            volume => { PlayerPrefs.SetFloat(PlayerPrefKey.SoundEffectVolume, volume); };

        private GameConfiguration()
        {
            Initial();
            Load();
        }

        public int GraphicQualityLevel
        {
            get { return _graphicQualityLevel; }
            set
            {
                if (_graphicQualityLevel == value) return;
                _graphicQualityLevel = value;
                OnGraphicQualityLevelChange(_graphicQualityLevel);
            }
        }

        public Resolution Resolution
        {
            get { return _resolution; }
            set
            {
                if (_resolution.Equals(value)) return;
                _resolution = value;
                OnResolutionChange(_resolution);
            }
        }

        public float BackgroundMusicVolume
        {
            get { return _backgroundMusicVolume; }
            set
            {
                if (Mathf.Approximately(_backgroundMusicVolume, value)) return;
                _backgroundMusicVolume = value;
                OnBackgroundMusicVolumeChange(_backgroundMusicVolume);
            }
        }

        public float SoundEffectVolume
        {
            get { return _soundEffectVolume; }
            set
            {
                if (Mathf.Approximately(_soundEffectVolume, value)) return;
                _soundEffectVolume = value;
                OnSoundEffectVolumeChange(_soundEffectVolume);
            }
        }

        public static GameConfiguration Get()
        {
            if (_instance == null)
                _instance = new GameConfiguration();
            return _instance;
        }

        private static void Initial()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefKey.BgmVolume))
                PlayerPrefs.SetFloat(PlayerPrefKey.BgmVolume, 0.5f);
            if (!PlayerPrefs.HasKey(PlayerPrefKey.SoundEffectVolume))
                PlayerPrefs.SetFloat(PlayerPrefKey.SoundEffectVolume, DefaultSoundEffectVolume);
            if (!PlayerPrefs.HasKey(PlayerPrefKey.ScreenWidth) || !PlayerPrefs.HasKey(PlayerPrefKey.ScreenHeight))
            {
                PlayerPrefs.SetInt(PlayerPrefKey.ScreenWidth, Screen.width);
                PlayerPrefs.SetInt(PlayerPrefKey.ScreenHeight, Screen.height);
            }
            if (!PlayerPrefs.HasKey(PlayerPrefKey.GraphicQuality))
                PlayerPrefs.SetInt(PlayerPrefKey.GraphicQuality, QualitySettings.GetQualityLevel());
        }

        private void Load()
        {
            BackgroundMusicVolume = PlayerPrefs.GetFloat(PlayerPrefKey.BgmVolume);
            SoundEffectVolume = PlayerPrefs.GetFloat(PlayerPrefKey.SoundEffectVolume);
            Resolution = new Resolution
            {
                height = PlayerPrefs.GetInt(PlayerPrefKey.ScreenHeight),
                width = PlayerPrefs.GetInt(PlayerPrefKey.ScreenWidth),
                refreshRate = Screen.currentResolution.refreshRate
            };
            GraphicQualityLevel = PlayerPrefs.GetInt(PlayerPrefKey.GraphicQuality);
        }
    }
}