using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Gui
{
    public class SceneFadeInOut : MonoBehaviour
    {
        private GUITexture _guiTexture;
        private bool _sceneEnding;
        private string _sceneName;
        private bool _sceneStarting = true;
        public float FadeSpeed = 1.5f;

        private void Awake()
        {
            _guiTexture = GetComponent<GUITexture>();
            _guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
            _guiTexture.enabled = true;
            GameObject.FindGameObjectWithTag(Tag.NetworkManager).GetComponent<NetworkManager>().SceneFade = this;
        }

        private void Update()
        {
            if (_sceneStarting)
                StartScene();
            else if (_sceneEnding)
                EndingScene();
        }

        private void FadeToClear()
        {
            _guiTexture.color = Color.Lerp(_guiTexture.color, Color.clear, FadeSpeed*Time.deltaTime);
        }


        private void FadeToBlack()
        {
            _guiTexture.color = Color.Lerp(_guiTexture.color, Color.black, FadeSpeed*Time.deltaTime);
        }

        private void StartScene()
        {
            FadeToClear();
            if (!(_guiTexture.color.a <= 0.05f)) return;
            _guiTexture.color = Color.clear;
            _guiTexture.enabled = false;
            _sceneStarting = false;
        }

        public void EndingScene()
        {
            FadeToBlack();
            if (!(_guiTexture.color.a >= 0.95f)) return;
            if (!string.IsNullOrEmpty(_sceneName))
                SceneManager.LoadScene(_sceneName);
            else
            {
                _guiTexture.enabled = false;
                _sceneEnding = false;
            }
        }

        public void EndScene(string sceneName)
        {
            _sceneName = sceneName;
            _guiTexture.enabled = true;
            _sceneEnding = true;
        }
    }
}