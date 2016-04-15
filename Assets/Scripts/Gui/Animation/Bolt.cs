using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class Bolt : MonoBehaviour, IDamage
    {
        private Vector3 _startPosition;
        public float Damage;
        public float Range;
        public float Speed;

        public float GetDamage()
        {
            return Damage;
        }

        public void Fire()
        {
            _startPosition = transform.position;
            GetComponent<Rigidbody>().velocity = transform.forward*Speed;
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        private void Update()
        {
            if (Vector3.Distance(GetComponent<Transform>().position, _startPosition) > Range)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}