using Assets.Scripts.Metadata;
using Assets.Scripts.Network;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class CanvasGroupFade : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private bool _isFadeIn;
        private bool _isFadeOut;
        public float FadeSpeed = 0.5f;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            GameObject.FindGameObjectWithTag(Tag.NetworkManager).GetComponent<NetworkManager>().FadeOut +=() =>
            {
                _canvasGroup.alpha = 0F;
            };
        }

        private void Update()
        {
            if (_isFadeIn)
                FadeIn();
            else if (_isFadeOut)
                FadeOut();
        }

        private void FadeOut()
        {
            var fade = FadeSpeed*Time.deltaTime;
            var newAlpha = _canvasGroup.alpha - fade;
            _canvasGroup.alpha = newAlpha < 0 ? 0 : newAlpha;
            if (_canvasGroup.alpha > 0) return;
            _isFadeOut = false;
        }

        private void FadeIn()
        {
            var fade = FadeSpeed*Time.deltaTime;
            var newAlpha = _canvasGroup.alpha + fade;
            _canvasGroup.alpha = newAlpha > 1F ? 1F : newAlpha;
            if (!Mathf.Approximately(_canvasGroup.alpha, 1F)) return;
            _isFadeIn = false;
        }

        public void StartFadeIn()
        {
            _isFadeIn = true;
            _canvasGroup.alpha = 0F;
        }

        public void StartFadeOut()
        {
            _isFadeOut = true;
            _canvasGroup.alpha = 1F;
        }
    }
}