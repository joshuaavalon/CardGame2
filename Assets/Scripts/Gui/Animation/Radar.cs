using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class Radar: MonoBehaviour
    {
        private TargetList _list;
        public float Distance;
        public int Group;
        public Transform Target;

        private void Start()
        {
            _list = GetComponentInParent<TargetList>();
        }

        private void Update()
        {
            Target = GetTarget();
        }

        private Transform GetTarget()
        {
            var position = GetComponent<Transform>().position;
            Transform ship = null;
            var minDistance = Distance + 1;
            foreach (var target in _list.ShipList)
            {
                if(target == null || Group == target.Group) continue;
                var targetTransform = target.GetComponent<Transform>();
                var distance = Vector3.Distance(position, targetTransform.position);
                if (distance > Distance || distance > minDistance) continue;
                minDistance = distance;
                ship = targetTransform;
            }
            return ship;
        }
    }
}
