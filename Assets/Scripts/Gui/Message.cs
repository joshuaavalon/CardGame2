using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class Message : MonoBehaviour
    {
        public Text Text;

        public void Start()
        {
            Text = gameObject.GetComponent<Text>();
        }

        public void Show(string message, float time = 1f)
        {
            Text.text = message;
            Text.enabled = true;
            Text.CrossFadeAlpha(0.0f, time , false);
            StartCoroutine(Wait(time));
            
        }

        private IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }

}
