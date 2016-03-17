using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class ButtonOnClickSound : MonoBehaviour
    {
        private AudioControl _audioControl;
        public SoundType AudioType;

        public void Start()
        {
            var audioObject = GameObject.FindGameObjectWithTag(Tag.Audio);
            _audioControl = audioObject.GetComponent<AudioControl>();
            GetComponent<Button>().onClick.AddListener(() => { _audioControl.GetAudioSource(AudioType).Play(); });
        }
    }
}