using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class FlyFoward : MonoBehaviour, IAimable
    {
        private Transform _target;
        public float Speed;
        public float Range;
        private Vector3 _startPosition;

        private void Update()
        {
            if (Vector3.Distance(GetComponent<Transform>().position, _startPosition) > Range)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Fire()
        {
            var component = GetComponent<Transform>();
            _startPosition = component.position;
            component.LookAt(_target);
            GetComponent<Rigidbody>().velocity = transform.forward * Speed;
        }
    }
}