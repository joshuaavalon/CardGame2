using Assets.Scripts.Audio;
using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class WrapDriveSound : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var audioObject = GameObject.FindGameObjectWithTag(Tag.Audio);
            var audioControl = audioObject.GetComponent<AudioControl>();
            audioControl.PlayAudioClip(AudioClipType.WrapDrive);
        }
    }
}
