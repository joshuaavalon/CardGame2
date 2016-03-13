using Assets.Scripts.Metadata;
using UnityEngine;

namespace Assets.Scripts
{
    public class AudioControl : MonoBehaviour
    {
        public AudioSource ButtonClick;
        public AudioSource BackgroundMusic;

        public void ButtonClickPlay()
        {
            ButtonClick.Play();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (GameObject.FindGameObjectsWithTag(Tag.Audio).Length > 1)
                DestroyImmediate(gameObject);
        }
    }
}