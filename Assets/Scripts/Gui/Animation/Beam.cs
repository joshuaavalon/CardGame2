using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class Beam : MonoBehaviour, IDamage
    {
        public float Damage;
        public float ShootTime;
        private float _timePassed;

        public float GetDamage()
        {
            return Damage;
        }

        public void Fire()
        {
            _timePassed = 0;
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        private void Update()
        {
            _timePassed += Time.deltaTime;
            if(_timePassed> ShootTime)
                Destroy(gameObject);
        }
    }
}
