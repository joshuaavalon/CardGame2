using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MoveForward : MonoBehaviour
    {
        public float Velocity;
        public GameObject WrapJump;
        public GameObject Canvas;
        public CanvasGroupFade Fade;
        public iTweenMoveOnPath PlayerPath;

        private void Start()
        {
            WrapJump.SetActive(true);
            StartCoroutine(Wait(5F));
            StartCoroutine(WaitCanves(3.5F));
        }

        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            WrapJump.SetActive(false);
            PlayerPath.enabled = true;
        }

        IEnumerator WaitCanves(float time)
        {
            yield return new WaitForSeconds(time);
            Canvas.SetActive(true);
            Fade.StartFadeIn();
        }
    }
}