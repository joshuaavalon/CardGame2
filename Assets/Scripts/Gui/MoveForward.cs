using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MoveForward : MonoBehaviour
    {
        public float Velocity;
        public GameObject WrapJump;
        public GameObject Canvas;

        private void Start()
        {
            WrapJump.SetActive(true);
            StartCoroutine(Wait(10F));
            StartCoroutine(WaitCanves(3.5F));
        }

        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            WrapJump.SetActive(false);
            Canvas.SetActive(true);
        }

        IEnumerator WaitCanves(float time)
        {
            yield return new WaitForSeconds(time);
            WrapJump.SetActive(false);
            Canvas.SetActive(true);
        }
    }
}