using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class MessageHandler : MonoBehaviour
    {
        public GameObject MessagePrefab;
        public Canvas Canvas;

        public void ShowMessage(string message, float time = 2f)
        {
            var gameMessage = Instantiate(MessagePrefab);
            gameMessage.transform.SetParent(Canvas.transform, false);
            var component = gameMessage.GetComponent<Message>();
            component.Show(message, time);
        }
    }
}