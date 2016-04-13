using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Configuration;
using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;

// ReSharper disable ConvertIfStatementToNullCoalescingExpression

namespace Assets.Scripts.Audio
{
    public class AudioControl : MonoBehaviour
    {
        private Dictionary<AudioClipType, AudioClip> _audioClips;
        private GameConfiguration _config;
        private List<AudioSource> _soundEffAudioSources;
        public AudioSource BackgroundMusic;
        public LabelAudioClip[] LabelAudioClips;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _config = GameConfiguration.Get();
            _config.OnBackgroundMusicVolumeChange += ChangeBackgroundMusicVolume;
            _config.OnSoundEffectVolumeChange += ChangeSoundEffectVolume;
            BackgroundMusic.volume = _config.BackgroundMusicVolume;
            if (GameObject.FindGameObjectsWithTag(Tag.Audio).Length > 1)
                DestroyImmediate(gameObject);
            _soundEffAudioSources = new List<AudioSource>();
            _audioClips = new Dictionary<AudioClipType, AudioClip>();
            foreach (var labelAudioClip in LabelAudioClips.Where(
                labelAudioClip => !_audioClips.ContainsKey(labelAudioClip.Label) && labelAudioClip.AudioClip != null))
            {
                _audioClips.Add(labelAudioClip.Label, labelAudioClip.AudioClip);
            }
        }

        private void Start()
        {
            PlayAudioClip(AudioClipType.Greetings);
        }

        public void PlayAudioClip(AudioClipType type, float volume = -1, bool loop = false, bool clearOther = false)
        {
            if (!_audioClips.ContainsKey(type)) return;
            if (volume < 0)
                volume = _config.SoundEffectVolume;
            if (clearOther)
                foreach (var audioSource in _soundEffAudioSources)
                {
                    DestroyImmediate(audioSource);
                }
            var playSource = _soundEffAudioSources.FirstOrDefault(audioSource => audioSource.isPlaying == false);
            if (playSource == null)
            {
                playSource = gameObject.AddComponent<AudioSource>();
                playSource.bypassEffects = true;
                playSource.bypassListenerEffects = true;
                playSource.bypassReverbZones = true;
                _soundEffAudioSources.Add(playSource);
            }
            playSource.clip = _audioClips[type];
            playSource.volume = volume;
            playSource.loop = loop;
            playSource.Play();
        }

        [Serializable]
        public struct LabelAudioClip
        {
            public AudioClipType Label;
            public AudioClip AudioClip;
        }

        private void ChangeSoundEffectVolume(float volume)
        {
            foreach (var audioSource in _soundEffAudioSources)
            {
                audioSource.volume = volume;
            }
        }

        private void ChangeBackgroundMusicVolume(float volume)
        {
            BackgroundMusic.volume = volume;
        }

        public void ResetVolume()
        {
            ChangeBackgroundMusicVolume(_config.BackgroundMusicVolume);
            ChangeSoundEffectVolume(_config.SoundEffectVolume);
        }
    }
}