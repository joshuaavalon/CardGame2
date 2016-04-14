using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class BoltWeapon : MonoBehaviour, IAimable
    {
        private float _coolDown;
        private Transform _target;
        public GameObject Bolt;
        public float Charge = 1f;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Fire()
        {
            if (_target == null) return;
            var bolt = Instantiate(Bolt);
            bolt.transform.position = GetComponent<Transform>().position;
            var aimable = bolt.GetInterface<IAimable>();
            aimable.SetTarget(_target);
            aimable.Fire();
        }

        private void Update()
        {
            if (_coolDown >= Charge)
            {
                Fire();
                _coolDown = 0;
            }
            else
                _coolDown += Time.deltaTime;
        }
    }
}