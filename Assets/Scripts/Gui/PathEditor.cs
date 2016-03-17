using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class PathEditor:MonoBehaviour
    {
        public Color Color = Color.white;
        public List<Transform> Path = new List<Transform>();
        private Transform[] _array;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color;
            _array = GetComponentsInChildren<Transform>();
            Path.Clear();
            foreach (var point in _array.Where(point => point!=transform))
            {
                Path.Add(point);
            }
            for (var i = 0; i < Path.Count; i++)
            {
                var position = Path[i].position;
                if (i==0) continue;
                var previousPosition = Path[i-1].position;
                Gizmos.DrawLine(previousPosition,position);
                Gizmos.DrawWireSphere(position, 1f);
            }
        }
    }
}
