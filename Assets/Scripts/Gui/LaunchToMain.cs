using System.Collections;
using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Gui
{
    public class LaunchToMain : MonoBehaviour
    {
        public MoveCamera Camera;
        public GameObject CameraPoint;
        public GameObject Engine;
        public MoveOnPath MoveObject;

        private void Start()
        {
            MoveObject.OnReach += Reach;
        }

        public void Launch()
        {
            Camera.Destination = CameraPoint.transform;
            Camera.TimeSpan = 0.3f;
            StartCoroutine(Wait());
            GameObject.FindGameObjectWithTag(Tag.Audio)
                .GetComponent<AudioControl>().GetAudioSource(SoundType.ActiveHyperDrive).Play();
            MoveObject.enabled = true;
            Engine.SetActive(true);
        }

        private void Reach()
        {
            SceneManager.LoadScene("Main");
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(2);
        }
    }
}