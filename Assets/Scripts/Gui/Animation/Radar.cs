using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class Radar : MonoBehaviour
    {
        private TargetList _list;
        public float Distance;
        public int Group;
        public Transform Target;
        public float SearchCoolDown = 1F;
        private float _timespan;

        private void Start()
        {
            _list = GetComponentInParent<TargetList>();
            _list.AddRadar(this);
        }

        private void Update()
        {
            _timespan += Time.deltaTime;
            if (!(_timespan > SearchCoolDown)) return;
            Target = GetTarget();
            _timespan = 0;
        }

        private Transform GetTarget()
        {
            var position = GetComponent<Transform>().position;
            Transform ship = null;
            var minDistance = Distance + 1;
            foreach (var target in _list.GetRadars(Group == 0 ? 1 : 0))
            {
                var targetTransform = target.GetComponent<Transform>();
                var distance = Vector3.Distance(position, targetTransform.position);
                if (distance > Distance || distance > minDistance) continue;
                minDistance = distance;
                ship = targetTransform;
            }
            return ship;
        }

        private void OnDestroy()
        {
            _list.RemoveRadar(this);
        }
    }
}