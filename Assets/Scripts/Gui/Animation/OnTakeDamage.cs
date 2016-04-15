using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class OnTakeDamage : MonoBehaviour
    {
        private Ship _parent;
        private void Start()
        {
            _parent = GetComponentInParent<Ship>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            var iDamage = collision.gameObject.GetInterface<IDamage>();
            if(iDamage ==null) return;
            _parent.TakeDamage(iDamage.GetDamage());
        }
    }
}
