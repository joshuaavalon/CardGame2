using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class Turret : MonoBehaviour
    {
        private float _coolDown;
        private Radar _radar;
        public GameObject Bolt;
        public float Charge = 1f;
        public Transform Gun;
        public float MaxmimumHorizontalAngle = 60;
        public float MaxmimumLowHorizontalAngle = 60;
        public float MaxmimumVerticalAngle = 30;
        public float MaxmimumLowVerticalAngle = 30;
        public float RotateSpeed = 1;
        public Transform Spwan;
        public float Damage;

        private void Start()
        {
            _radar = GetComponentInParent<Radar>();
        }

        private void Update()
        {
            if (_radar.Target == null) return;
            var toTarget = Quaternion.LookRotation(_radar.Target.position - Gun.transform.position, Gun.transform.up);
            Gun.transform.rotation = Quaternion.RotateTowards(Gun.transform.rotation, toTarget,
                Time.deltaTime*RotateSpeed);
            Gun.transform.localRotation = new Quaternion(Gun.transform.localRotation.x, Gun.transform.localRotation.y,
                Quaternion.identity.z, Gun.transform.localRotation.w);
            var angleX = Gun.transform.localEulerAngles.x.ClampAngle(-MaxmimumVerticalAngle, -MaxmimumLowVerticalAngle);
            var angleY = Gun.transform.localEulerAngles.y.ClampAngle(MaxmimumLowHorizontalAngle, MaxmimumHorizontalAngle);
            Gun.transform.localEulerAngles = new Vector3(angleX, angleY, Gun.transform.localEulerAngles.z);

            if (_coolDown >= Charge)
            {
                var bolt = Instantiate(Bolt);
                bolt.transform.position = Spwan.position;
                bolt.transform.rotation = Spwan.rotation;
                _coolDown = 0;
                var iDamage = bolt.GetComponent<IDamage>();
                if(Damage > 0)
                    iDamage.SetDamage(Damage);
                var missile = iDamage as Missile;
                if (missile != null)
                    missile.Target = _radar.Target;
                var beam = iDamage as Beam;
                if (beam != null)
                    beam.transform.parent = Spwan;
                iDamage.Fire();
            }
            else
                _coolDown += Time.deltaTime;
        }
    }
}