using System.Collections;
using Assets.Scripts.Gui.Animation;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MoveForward : MonoBehaviour
    {
        public GameObject Canvas;
        public CanvasGroupFade Fade;
        public int Point;

        private void Awake()
        {
            var nodes = GetComponent<iTweenPath>().Nodes;
            var factory = GetComponentInParent<SpwanFactory>();
            nodes.Clear();
            nodes.AddRange(factory.GetRandomPath(Point, Point));
            nodes.Add(GetComponentInParent<iTweenPath>().transform.position);
        }

        private void Start()
        {
            StartCoroutine(WaitCanves(3.5F));
        }

        IEnumerator WaitCanves(float time)
        {
            yield return new WaitForSeconds(time);
            Canvas.SetActive(true);
            Fade.StartFadeIn();
        }
    }
}