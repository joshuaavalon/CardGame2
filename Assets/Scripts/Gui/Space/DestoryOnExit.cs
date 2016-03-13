using UnityEngine;

namespace Assets.Scripts.Gui.Space
{
    public class DestoryOnExit : MonoBehaviour {

        private void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}
