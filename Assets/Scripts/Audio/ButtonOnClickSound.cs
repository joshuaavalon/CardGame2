using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Audio
{
    public class ButtonOnClickSound : MonoBehaviour
    {
        private AudioControl _audioControl;
        public AudioClipType AudioType;

        public void Start()
        {
            var audioObject = GameObject.FindGameObjectWithTag(Tag.Audio);
            _audioControl = audioObject.GetComponent<AudioControl>();
            GetComponent<Button>().onClick.AddListener(() => { _audioControl.PlayAudioClip(AudioType); });
        }
    }
}