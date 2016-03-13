using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.DeckEdit;
using Assets.Scripts.Gui.Menu;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class NetworkManager : MonoBehaviour
    {
        public Button CancelButton;
        public Button CreateButton;
        public Button NewButton;
        public GameObject RoomListItem;
        public Text RoomNameInputField;
        public GameObject ScrollViewContent;
        private bool _showWarning=true;
        private static NetworkManager _single;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings("0.5");
            DeckManager.LoadToDeck();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnJoinedLobby()
        {
            if (NewButton != null)
                NewButton.interactable = Deck.Get().EnoughCard();
            if (CreateButton != null)
                CreateButton.interactable = true;
        }

        private void OnReceivedRoomListUpdate()
        {
            if (ScrollViewContent != null)
            {
                foreach (Transform child in ScrollViewContent.transform)
                {
                    Destroy(child.gameObject);
                }
                foreach (var room in PhotonNetwork.GetRoomList())
                {
                    var roomListItem = Instantiate(RoomListItem);
                    var texts = roomListItem.GetComponentsInChildren<Text>();
                    texts[0].text = room.name;
                    texts[1].text = room.playerCount + " / 2"+ (room.playerCount==2? " <b><color=#AA3330FF>FULL</color></b>":"");
                    roomListItem.transform.SetParent(ScrollViewContent.transform);
                    var button = roomListItem.GetComponentsInChildren<Button>()[1];
                    button.interactable = room.playerCount < 2;
                    button.onClick.AddListener(() => { PhotonNetwork.JoinRoom(room.name); });
                    roomListItem.transform.localScale = Vector3.one;
                }
            }
        }

        private void OnGUI()
        {
            //GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            if (_showWarning && !Deck.Get().EnoughCard())
                GUILayout.Label("Card count in your deck is not valid.\nEdit your deck before starting a game.");
        }

        public void CreateRoom()
        {
            var roomName = RoomNameInputField.text;
            if (string.IsNullOrEmpty(roomName) || IsRoomExists(roomName) || roomName.Length > 20) return;

            if (PhotonNetwork.CreateRoom(RoomNameInputField.text, new RoomOptions {maxPlayers = 2, isVisible = true},
                null))
            {
                CreateButton.interactable = false;
                CancelButton.interactable = true;
                CreateButton.GetComponent<ChangeMenu>().Change();
            }
            else
            {
                CreateButton.interactable = true;
            }
        }

        private void OnEnterEdit()
        {
            SceneManager.LoadScene("Edit");
        }

        private void OnLeftRoom()
        {
            if (CancelButton != null)
            {
                CancelButton.interactable = false;
                CancelButton.GetComponent<ChangeMenu>().Change();
            }
            else
            {
                SceneManager.LoadScene("Menu");
                DestroyImmediate(gameObject);
            }
        }

        private void OnJoinedRoom()
        {
            if (!PhotonNetwork.isMasterClient)
                SceneManager.LoadScene("Main");
        }

        private void OnPhotonPlayerConnected()
        {
            SceneManager.LoadScene("Main");
        }

        private void OnPhotonPlayerDisconnected()
        {
            if (!Game.End)
                PhotonNetwork.LeaveRoom();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private static bool IsRoomExists(string roomName)
        {
            return PhotonNetwork.GetRoomList().Any(room => room.name.Equals(roomName));
        }

        private void OnLevelWasLoaded(int level)
        {
            _showWarning = level == 0;
        }
    }
}