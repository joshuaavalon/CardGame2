using System;
using System.Linq;
using Assets.Scripts.Audio;
using Assets.Scripts.Core;
using Assets.Scripts.DeckEdit;
using Assets.Scripts.Gui;
using Assets.Scripts.Gui.Menu;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Network
{
    public class NetworkManager : MonoBehaviour
    {
        private AudioControl _audioControl;
        private bool _showWarning = true;
        public Button CancelButton;
        public Button CreateButton;
        public Action FadeOut = () => { };
        public LaunchToMain Launch;
        public Button NewButton;
        public GameObject RoomListItem;
        public Text RoomNameInputField;
        public SceneFadeInOut SceneFade;
        public GameObject ScrollViewContent;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings("1.0");
            DeckHandler.LoadFromSaveToDeck();
            _audioControl = GameObject.FindGameObjectWithTag(Tag.Audio).GetComponent<AudioControl>();
            ButtonCheck();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnJoinedLobby()
        {
            ButtonCheck();
        }

        private void OnReceivedRoomListUpdate()
        {
            if (ScrollViewContent == null) return;
            foreach (Transform child in ScrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (var room in PhotonNetwork.GetRoomList())
            {
                var roomListItem = Instantiate(RoomListItem);
                var texts = roomListItem.GetComponentsInChildren<Text>();
                texts[0].text = room.name;
                texts[1].text = room.playerCount + " / 2" +
                                (room.playerCount == 2 ? " <b><color=#AA3330FF>FULL</color></b>" : "");
                roomListItem.transform.SetParent(ScrollViewContent.transform, false);
                var button = roomListItem.GetComponentsInChildren<Button>()[1];
                if (room.playerCount < 2)
                {
                    button.interactable = true;
                    var currentRoom = room;
                    button.onClick.AddListener(() => { PhotonNetwork.JoinRoom(currentRoom.name); });
                }
                else
                {
                    Destroy(button.gameObject);
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
            if (string.IsNullOrEmpty(roomName) || IsRoomExists(roomName) || roomName.Length > 20)
            {
                _audioControl.PlayAudioClip(AudioClipType.AccessDenied);
                return;
            }

            if (PhotonNetwork.CreateRoom(RoomNameInputField.text, new RoomOptions {maxPlayers = 2, isVisible = true},
                null))
            {
                CreateButton.interactable = false;
                CancelButton.interactable = true;
                _audioControl.PlayAudioClip(AudioClipType.PrepareHyperDrive);
                CreateButton.GetComponent<ChangeMenu>().Change();
            }
            else
            {
                GameObject.FindGameObjectWithTag(Tag.MessageHandler).GetComponent<MessageHandler>().ShowMessage("Failed to create room");
                CreateButton.interactable = true;
            }
        }

        private void OnLeftRoom()
        {
            if (CancelButton != null && Launch.Engine.activeInHierarchy == false)
            {
                CancelButton.interactable = false;
                CancelButton.GetComponent<ChangeMenu>().Change();
            }
            else
            {
                FadeOut();
                SceneFade.EndScene("Menu");
                DestroyImmediate(gameObject);
            }
        }

        private void OnJoinedRoom()
        {
            if (!PhotonNetwork.isMasterClient)
                Launch.Launch();
        }

        private void OnPhotonPlayerConnected()
        {
            Launch.Launch();
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
            if (level != 0) return;
            ButtonCheck();
        }

        public void ButtonCheck()
        {
            var interactable = Deck.Get().EnoughCard() && PhotonNetwork.insideLobby;
            if (NewButton != null)
                NewButton.interactable = interactable;
            if (CreateButton != null)
                CreateButton.interactable = interactable;
        }
    }
}