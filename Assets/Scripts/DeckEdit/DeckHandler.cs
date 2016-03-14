using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.Gui;
using Assets.Scripts.Metadata;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;
using Card = Assets.Scripts.Gui.Card;

namespace Assets.Scripts.DeckEdit
{
    public class DeckHandler : MonoBehaviour
    {
        private IDictionary<string, int> _cardList;
        private IDictionary<string, CardThumb> _cardObjects;
        private IList<string> _cardPool;
        private Deck _deck;
        private bool _initial;
        private MessageHandler _messageHandler;
        public GameObject CardImagePrefab;
        public Transform CardList;
        public Transform CardPool;
        public GameObject CardThumbPrefab;
        public Button FinishButton;
        public GameObject HoverCardPrefab;
        public Image ProgressBar;
        public Text ProgressText;

        private void Start()
        {
            _messageHandler = GameObject.FindGameObjectWithTag(Tag.MessageHandler).GetComponent<MessageHandler>();
            _deck = Deck.Get();
            _cardPool = new List<string>();
            var cards = Resources.LoadAll<Card>("");
            foreach (var card in cards.Where(card => !card.name.Contains("e")))
            {
                _cardPool.Add(card.name);
                AddCardToPool(card.name);
            }
            _cardList = new Dictionary<string, int>();
            _cardObjects = new Dictionary<string, CardThumb>();
            FinishButton.interactable = false;
            _initial = true;
            Load();
        }

        private void AddCardToPool(string cardName)
        {
            var card = Resources.Load<GameObject>(cardName);
            var cardCom = card.GetComponent<Card>();
            var cardImage = Instantiate(CardImagePrefab);
            cardImage.name = cardName;
            cardImage.GetComponent<Image>().sprite = cardCom.Image;
            cardImage.transform.SetParent(CardPool, false);
            cardImage.GetComponent<Button>().onClick.AddListener(() => { AddCard(cardName); });
        }

        private void AddCardsToList()
        {
            foreach (var keyPair in _cardList)
            {
                AddCardToList(keyPair.Key, keyPair.Value);
            }
        }

        private void AddCardToList(string cardName, int count = 1)
        {
            var card = Resources.Load<GameObject>(cardName);
            var cardCom = card.GetComponent<Card>();
            var cardThumb = Instantiate(CardThumbPrefab).GetComponent<CardThumb>();
            cardThumb.name = cardName;
            cardThumb.Image.sprite = cardCom.Thumbnail;
            cardThumb.transform.SetParent(CardList, false);
            cardThumb.Count.text = count.ToString();
            _cardObjects.Add(new KeyValuePair<string, CardThumb>(cardName, cardThumb));
            cardThumb.GetComponent<Button>().onClick.AddListener(() => { RemoveCard(cardName); });
        }

        private void AddCard(string cardName)
        {
            if (TotalCardCount() >= _deck.RequireCard())
            {
                if (_messageHandler != null)
                    _messageHandler.ShowMessage("You cannot have more than " + _deck.RequireCard() + " cards!");
                return;
            }
            if (_cardList.ContainsKey(cardName))
            {
                if (_cardList[cardName] >= _deck.MaxOfSingle())
                {
                    if (_messageHandler != null)
                        _messageHandler.ShowMessage("You cannot have more than " + _deck.MaxOfSingle() +
                                                    " copies of a card!");
                    return;
                }
                _cardList[cardName]++;
                _cardObjects[cardName].Count.text = _cardList[cardName].ToString();
            }
            else
            {
                _cardList.Add(new KeyValuePair<string, int>(cardName, 1));
                AddCardToList(cardName);
            }
            UpdateProgress();
        }

        private void RemoveCard(string cardName)
        {
            if (!_cardList.ContainsKey(cardName)) return;
            if (_cardList[cardName] > 1)
            {
                _cardList[cardName]--;
                _cardObjects[cardName].Count.text = _cardList[cardName].ToString();
            }
            else
            {
                _cardList.Remove(cardName);
                Destroy(_cardObjects[cardName].gameObject);
                _cardObjects.Remove(cardName);
            }
            UpdateProgress();
        }

        private int TotalCardCount()
        {
            return _cardList.Values.Sum();
        }

        private void UpdateProgress()
        {
            var total = TotalCardCount();
            var isEnough = total == _deck.RequireCard();
            ProgressText.text = (isEnough ? "" : "<color=red>") + total + (isEnough ? "" : "</color>")
                                + " / " + _deck.RequireCard();
            var percentage = (float) total/_deck.RequireCard();
            ProgressBar.fillAmount = percentage > 1 ? 1 : percentage;
            FinishButton.interactable = TotalCardCount() == _deck.RequireCard();
        }

        private void LoadToDeck()
        {
            _deck.Clear();
            var list = new List<string>();
            foreach (var keypair in _cardList)
            {
                for (var i = 0; i < keypair.Value; i++)
                    list.Add(keypair.Key);
            }
            list.Sort();
            foreach (var card in list)
            {
                _deck.Add(card);
            }
            PlayerPrefsX.SetStringArray(PlayerPrefKey.CardList, list.ToArray());
        }

        public static void LoadFromSaveToDeck()
        {
            var deckList = PlayerPrefsX.GetStringArray(PlayerPrefKey.CardList);
            var deck = Deck.Get();
            deck.Clear();
            foreach (var card in deckList)
                deck.Add(card);
        }

        private void Load()
        {
            ClearUp();
            var savedList = PlayerPrefsX.GetStringArray(PlayerPrefKey.CardList);
            if (savedList != null)
                foreach (var card in savedList.Where(card => _cardPool.Contains(card)))
                {
                    if (_cardList.ContainsKey(card))
                    {
                        if (_cardList[card] <= 0)
                            _cardList[card] = 1;
                        else if (_cardList[card] >= _deck.MaxOfSingle())
                            _cardList[card] = _deck.MaxOfSingle();
                        else
                            _cardList[card]++;
                    }
                    else
                        _cardList.Add(new KeyValuePair<string, int>(card, 1));
                }
            AddCardsToList();
            UpdateProgress();
        }

        private void ClearUp()
        {
            if (_cardList != null)
                _cardList.Clear();

            if (_cardObjects == null) return;
            foreach (var card in _cardObjects.Values)
            {
                Destroy(card.gameObject);
            }
            _cardObjects.Clear();
        }

        public void OnEnable()
        {
            if (_initial)
                Load();
        }

        public void OnDisable()
        {
            LoadToDeck();
        }
    }
}