using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class Missile : MonoBehaviour, IDamage
    {
        private Vector3 _startPosition;
        private float _timePassed;
        public float Damage;
        public GameObject Exposion;
        public float GuideTime;
        public float Range;
        public float RotateSpeed;
        public float Speed = 50;
        public Transform Target;

        public float GetDamage()
        {
            return Damage;
        }

        public void Fire()
        {
            _startPosition = transform.position;
            _timePassed = 0;
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        private void Update()
        {
            if (Vector3.Distance(GetComponent<Transform>().position, _startPosition) > Range)
                Destroy(gameObject);
            if (_timePassed < GuideTime && Target != null)
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(Target.position - transform.position), Time.deltaTime*RotateSpeed);
            transform.position += transform.forward*Speed*Time.deltaTime;
            _timePassed += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Exposion == null) return;
            var exposion = Instantiate(Exposion);
            exposion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}