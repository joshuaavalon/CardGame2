using System;
using System.Collections;
using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Card = Assets.Scripts.Gui.Card;

namespace Assets.Scripts.DeckEdit
{
    public class DeckManager : MonoBehaviour
    {
        private Transform[] _gameObjects;

        private void Start()
        {
            //PlayerPrefs.DeleteAll();
            if (PhotonNetwork.connected)
                PhotonNetwork.Disconnect();

            //Chosen deck
            var sArray = PlayerPrefsX.GetStringArray("CardString");
            var textObj = GameObject.Find("Card Number");
            if (textObj != null)
            {
                var txt = textObj.GetComponent<Text>();
                txt.text = sArray.Length + "/" + Deck.Get().RequireCard();
                ;
            }
            if (sArray == null) return;
            foreach (var t in sArray)
            {
                //Debug.Log("List from PrefX: "+t);
                AddPrefab(t, "Chosen");
            }

            //Choice deck
            Resources.LoadAll<GameObject>("");
            var cardGameObject = Resources.FindObjectsOfTypeAll(typeof (Card));
            var cardname = new string[cardGameObject.Length];
            for (var i = 0; i < cardGameObject.Length; i++)
            {
                cardname[i] = cardGameObject[i].name;
            }
            //var c = cardname.Except(sArray).ToArray();
            foreach (var variable in cardname)
            {
                if (variable.Contains("(Clone)")) continue;
                AddPrefab(variable, "Card Choice");
            }
        }


        public void OnEnterEdit()
        {
            SceneManager.LoadScene("Edit");
            DestroyImmediate(GameObject.FindGameObjectWithTag(Tag.NetworkManager));
        }


        public void BackToMenu()
        {
            SetDeckList();
            if (Deck.Get().RequireCard() != GetDeckList().Length)
            {
                GameObject.Find("Warning").GetComponent<Text>().text = "Please make sure you have " +
                                                                       Deck.Get().RequireCard() + " cards.";
                return;
            }
            if (PhotonNetwork.connected)
                PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Menu");
        }

        private void AddPrefab(string cardName, string destination)
        {
            if (cardName.Contains("e")) return;
            var panel = GameObject.Find(destination);
            if (panel == null) return;
            var obj = (GameObject) Instantiate(Resources.Load(cardName));
            var drag = obj.AddComponent<Draggable>();
            drag.CardView = obj.GetComponent<Card>().Image;
            obj.transform.SetParent(panel.transform, false);
            obj.GetComponent<Image>().sprite = obj.GetComponent<Card>().Image;
        }

        public void SetDeckList()
        {
            var deckList = new ArrayList();
            var panel = GameObject.Find("Chosen");
            _gameObjects = panel.GetComponentsInChildren<Transform>(true);
            var result = new string[_gameObjects.Length - 1];
            if (_gameObjects != null)
            {
                foreach (var ob in _gameObjects)
                {
                    if (!ob.name.Equals("Chosen"))
                    {
                        var index = ob.name.IndexOf("(Clone)", StringComparison.Ordinal);
                        deckList.Add(ob.name.Substring(0, index));
                        //Debug.Log("SetDeck: "+ ob.name.Substring(0, index));
                    }
                }
                result = (string[]) deckList.ToArray(typeof (string));
            }
            PlayerPrefsX.SetStringArray("CardString", result);
        }

        private static string[] GetDeckList()
        {
            return PlayerPrefsX.GetStringArray("CardString");
        }

        public static void LoadToDeck()
        {
            var deckList = GetDeckList();
            var deck = Deck.Get();
            deck.Clear();
            foreach (var card in deckList)
                deck.Add(card);
        }

        public void UpdateNumber(int change)
        {
            var textObj = GameObject.Find("Card Number");
            if (textObj == null) return;
            var txt = textObj.GetComponent<Text>();
            var index = txt.text.IndexOf("/", StringComparison.Ordinal);
            var tempStr = txt.text.Substring(0, index);
            var x = int.Parse(tempStr);
            txt.text = x + change + "/" + Deck.Get().RequireCard();
            ;
        }
    }
}