using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class AutoAudio : MonoBehaviour
    {
        private AudioControl _audioControl;
        public AudioClipType AudioType;
        public bool PlayOnAwake;
        public bool PlayOnStart;
        public bool PlayOnDestroy;
        public bool PlayOnEnable;
        public bool PlayOnDisable;

        private void Awake()
        {
            var audioObject = GameObject.FindGameObjectWithTag(Tag.Audio);
            _audioControl = audioObject.GetComponent<AudioControl>();
            if(PlayOnAwake)
                _audioControl.PlayAudioClip(AudioType);
        }
        
        private void Start()
        {
            if (PlayOnStart)
                _audioControl.PlayAudioClip(AudioType);
        }

        private void OnDestroy()
        {
            if (PlayOnDestroy)
                _audioControl.PlayAudioClip(AudioType);
        }

        private void OnEnable()
        {
            if (PlayOnEnable)
                _audioControl.PlayAudioClip(AudioType);
        }

        private void OnDisable()
        {
            if (PlayOnDisable)
                _audioControl.PlayAudioClip(AudioType);
        }
    }
}