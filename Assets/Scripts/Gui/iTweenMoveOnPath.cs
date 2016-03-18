using System;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    // ReSharper disable once InconsistentNaming
    public class iTweenMoveOnPath : MonoBehaviour
    {
        public iTweenPath Path;
        public Action OnReach = () => { };
        public float Delay = 0f;
        public float Time = 1f;

        private void OnEnable()
        {
            iTween.MoveTo(gameObject, iTween.Hash("path", Path.nodes.ToArray(),
                "time", Time, "easetype", iTween.EaseType.easeInOutSine, "orienttopath", true
                , "oncomplete", "OnComplete", "delay", Delay));
        }

        public void OnComplete()
        {
            OnReach();
        }
    }
}