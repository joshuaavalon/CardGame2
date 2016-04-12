using System.Collections;
using Assets.Scripts.Audio;
using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class LaunchToMain : MonoBehaviour
    {
        public MoveCamera Camera;
        public GameObject CameraPoint;
        public GameObject Engine;
        public GameObject WrapEffect;
        public SceneFadeInOut SceneFadeInOut;

        public void Launch()
        {
            Camera.Destination = CameraPoint.transform;
            Camera.TimeSpan = 1f;
            GameObject.FindGameObjectWithTag(Tag.Audio)
                .GetComponent<AudioControl>().PlayAudioClip(AudioClipType.ActiveHyperDrive);
            WrapEffect.SetActive(true);
            Engine.SetActive(true);
            StartCoroutine(Wait(3.5F));
        }

        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            SceneFadeInOut.EndScene("Main");
        }
    }
}