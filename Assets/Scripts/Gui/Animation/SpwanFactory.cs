using System.Collections.Generic;
using Assets.Scripts.Configuration;
using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class SpwanFactory : MonoBehaviour
    {
        public GameObject[] Group0Ship;
        public GameObject[] Group1Ship;
        public int MaxPoint = 5;
        public int MinPoint = 3;
        public float Radius = 200;
        public int ShipCount = 5;
        private TargetList _targetList;
        public float SearchCoolDown = 1F;
        private float _timespan;

        private void Start()
        {
            ShipCount = GameConfiguration.Get().NumberOfShip;
            _targetList = GetComponent<TargetList>();
        }

        private void Update()
        {
            _timespan += Time.deltaTime;
            if (!(_timespan > SearchCoolDown)) return;
            if (_targetList.GetRadars(0).Count < ShipCount)
                SpwanShip(true);
            if (_targetList.GetRadars(1).Count < ShipCount)
                SpwanShip(false);
        }

        private void SpwanShip(bool isGroupZero)
        {
            var shipPrefab = RandomShip(isGroupZero);
            var ship = Instantiate(shipPrefab);
            ship.transform.position = GetRandomPoint();
            ship.transform.rotation = Random.rotation;
            var nodes = ship.GetComponent<iTweenPath>().Nodes;
            nodes.Clear();
            nodes.AddRange(GetRandomPath(MaxPoint, MinPoint));
            nodes.Add(ship.transform.position);
            ship.GetComponent<Radar>().Group = isGroupZero ? 0 : 1;
            ship.transform.parent = transform;
        }

        private Vector3 GetRandomPoint()
        {
            return transform.position+Random.insideUnitSphere*Radius;
        }

        public List<Vector3> GetRandomPath(int maxPoint, int minPoint=1)
        {
            var nodes = new List<Vector3>();
            var numOfPoint = Random.Range(minPoint, maxPoint + 1);
            for (var i = 0; i < numOfPoint; i++)
            {
                nodes.Add(GetRandomPoint());
            }
            return nodes;
        }

        private GameObject RandomShip(bool isGroupZero)
        {
            return isGroupZero
                ? Group0Ship[Random.Range(0, Group0Ship.Length)]
                : Group1Ship[Random.Range(0, Group1Ship.Length)];
        }
    }
}