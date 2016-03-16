using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MoveCamera : MonoBehaviour
    {
        private Vector3 _previousPosition;
        public Camera Camera;
        public Transform Destination;
        public float Speed = 0.1f;
        private const float Threshold = 0.015f;
        public float Zoom = 1.0f;

        private void Start()
        {
            _previousPosition = transform.position;
        }


        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, Destination.position, Speed* Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Destination.rotation, Speed* Time.deltaTime);
            var velocity = Vector3.Magnitude(transform.position - _previousPosition);
            Camera.fieldOfView = 60 + (velocity > 0.01 ? velocity : 0)*Zoom;
            _previousPosition = transform.position;

        }
    }
}