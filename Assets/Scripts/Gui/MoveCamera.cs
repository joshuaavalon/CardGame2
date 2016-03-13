using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MoveCamera : MonoBehaviour
    {
        private Vector3 _previousPosition;
        public Camera Camera;
        public Transform Destination;
        public float Speed = 0.1f;
        public float Zoom = 1.0f;

        private void Start()
        {
            _previousPosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, Destination.position, Speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Destination.rotation, Speed);
            var velocity = Vector3.Magnitude(transform.position - _previousPosition);
            Camera.fieldOfView = 60 + (velocity > 0.01 ? velocity : 0)*Zoom;
            _previousPosition = transform.position;
        }
    }
}