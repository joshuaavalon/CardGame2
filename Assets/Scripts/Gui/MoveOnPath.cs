using System;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MoveOnPath : MonoBehaviour
    {
        private const float ReachDistance = 3.0f;
        private int _current;
        public Action OnReach = () => { };
        public PathEditor Path;
        public float RotationSpeed = 5.0f;
        public float Speed;

        private void Update()
        {
            if (_current >= Path.Path.Count) return;
            var distance = Vector3.Distance(Path.Path[_current].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, Path.Path[_current].position,
                Time.deltaTime*Speed);
            var difference = Path.Path[_current].position - transform.position;
            var rotation = Quaternion.LookRotation(difference);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*RotationSpeed);
            if (distance <= ReachDistance)
                _current++;
            if (_current >= Path.Path.Count)
                OnReach();
        }
    }
}