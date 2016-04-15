using System;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    // ReSharper disable once InconsistentNaming
    public class iTweenMoveOnPath : MonoBehaviour
    {
        public iTweenPath Path;
        public float Delay = 0f;
        public float Speed = 1f;
        public float Looktime = 5f;
        public iTween.EaseType EaseType = iTween.EaseType.easeInOutSine;
        public iTween.LoopType LoopType = iTween.LoopType.none;
        public bool Orienttopath = false;

        private void OnEnable()
        {
            iTween.MoveTo(gameObject, iTween.Hash("path", Path.Nodes.ToArray(),
                "speed", Speed, "easetype", EaseType, "orienttopath", Orienttopath, 
                "delay", Delay, "looptype", LoopType, "looktime", Looktime));
        }
    }
}