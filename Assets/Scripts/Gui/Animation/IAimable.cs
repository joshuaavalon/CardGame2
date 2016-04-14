using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public interface IAimable
    {
        void SetTarget(Transform target);
        void Fire();
    }
}
