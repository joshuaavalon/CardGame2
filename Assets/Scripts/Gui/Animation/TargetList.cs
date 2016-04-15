using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class TargetList : MonoBehaviour
    {
        public Radar[] ShipList;

        private void Update()
        {
            ShipList = GetComponentsInChildren<Radar>();
        }
    }
}