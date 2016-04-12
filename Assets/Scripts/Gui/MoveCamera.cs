using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MoveCamera : MonoBehaviour
    {
        private float _i;
        private Vector3 _previous;
        public Camera Camera;
        public Transform Destination;
        public float TimeSpan = 5f;
        public float Zoom = 1.0f;

        private void Start()
        {
            _previous = transform.position;
        }

        private void Update()
        {
            if (transform.position == Destination.position)
                return;
            if (_previous != Destination.position)
            {
                _i = 0;
                _previous = Destination.position;
            }
            var rate = 1 / TimeSpan;
            _i += Time.deltaTime * TimeSpan;
            transform.position = Vector3.Lerp(transform.position, Destination.position, _i);
            transform.rotation = Quaternion.Slerp(transform.rotation, Destination.rotation, _i);
            if (transform.position != Destination.position) return;
            transform.SetParent(Destination);
        }
    }
}