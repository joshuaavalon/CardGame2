using UnityEngine;

namespace Assets.Scripts.Gui.Space
{
    public class RandomRotator : MonoBehaviour {

        public float Tumble;
        private void Start()
        {
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Tumble;
        }
    }
}
