using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class ButtonOnClickSound : MonoBehaviour
    {
        public void Start()
        {
            var network = GameObject.FindGameObjectWithTag(Tag.Audio);
            if (network == null) return;
            var ctrl = network.GetComponent<AudioControl>();
            GetComponent<Button>().onClick.AddListener(() => { ctrl.ButtonClickPlay(); });
        }
    }
}