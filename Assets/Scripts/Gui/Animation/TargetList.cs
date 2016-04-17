using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class TargetList : MonoBehaviour
    {
        //public Radar[] ShipList;
        public Dictionary<int, IList<Radar>> ShipList;

        private void Start()
        {
            ShipList = new Dictionary<int, IList<Radar>>();
            var list = GetComponentsInChildren<Radar>();
            foreach (var radar in list)
            {
                AddRadar(radar);
            }
        }

        public void AddRadar(Radar radar)
        {
            if (ShipList.ContainsKey(radar.Group))
                ShipList[radar.Group].Add(radar);
            else
                ShipList.Add(radar.Group, new List<Radar> {radar});
        }

        public void RemoveRadar(Radar radar)
        {
            if (ShipList.ContainsKey(radar.Group))
                ShipList[radar.Group].Remove(radar);
        }

        public IList<Radar> GetRadars(int group)
        {
            return ShipList.ContainsKey(group) ? ShipList[group] : new List<Radar>();
        }
    }
}