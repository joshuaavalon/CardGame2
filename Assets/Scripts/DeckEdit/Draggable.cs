using System;
using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Card = Assets.Scripts.Gui.Card;

namespace Assets.Scripts.DeckEdit
{
    public class Draggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler, IPointerClickHandler
    {
        private GameObject _clone;
        private string _destination;

        private Transform[] _gameObjects;
        private string _name;
        private GameObject _placeholder;
        public Sprite CardView;
        public Transform ParentToReturnTo;
        public Transform PlaceholderParent;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _placeholder = new GameObject();
            _placeholder.transform.SetParent(transform.parent);
            var le = _placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;
            _name = transform.parent.name;
            _placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

            _clone = (GameObject) Instantiate(gameObject, transform.position, transform.rotation);
            _clone.name = gameObject.name;

            ParentToReturnTo = transform.parent;
            PlaceholderParent = ParentToReturnTo;

            transform.SetParent(transform.parent.tag.Equals("ChoiceDeck")
                ? transform.parent.parent.parent.parent
                : transform.parent.parent.parent);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            if (_placeholder.transform.parent != PlaceholderParent)
                _placeholder.transform.SetParent(PlaceholderParent);
            var newSiblingIndex = PlaceholderParent.childCount;
            _placeholder.transform.SetSiblingIndex(newSiblingIndex + 1);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerEnter == null)
            {
                transform.SetParent(GameObject.FindGameObjectWithTag("ChoiceDeck").transform);
                return;
            }
            _destination = eventData.pointerEnter.name;
            //handle drag area that is a card
            if (_destination != "Panel" && _destination != "Container")
            {
                if (eventData.pointerEnter.GetComponent<Card>() != null)
                {
                    if (_name == "Card Choice" && eventData.pointerEnter.transform.parent.name == "Chosen" &&
                        !(eventData.pointerEnter.transform.parent.GetComponentsInChildren<Transform>(true).Length - 1 >=
                          Deck.Get().RequireCard()) && CheckDuplicate(name))
                    {
                        _clone.transform.SetParent(GameObject.FindGameObjectWithTag("ChoiceDeck").transform);
                        _clone.transform.localScale = new Vector3(1, 1, 1);
                        UpdateNumber(1);
                        transform.SetParent(eventData.pointerEnter.transform.parent);
                    }
                    else if (_name == "Chosen" && eventData.pointerEnter.transform.parent.name == "Card Choice")
                    {
                        UpdateNumber(-1);
                        Destroy(_clone);
                        Destroy(gameObject);
                    }
                    else
                    {
                        transform.SetParent(ParentToReturnTo);
                        Destroy(_clone);
                    }
                }
                else
                {
                    transform.SetParent(ParentToReturnTo);
                    Destroy(_clone);
                }
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                Destroy(_placeholder);
                return;
            }

            //handle drag area that is choice or chosen
            if (_name == "Card Choice" && _destination == "Panel" &&
                !(eventData.pointerEnter.GetComponentsInChildren<Transform>(true).Length - 1 >= Deck.Get().RequireCard()) &&
                CheckDuplicate(name))
            {
                _clone.transform.SetParent(GameObject.FindGameObjectWithTag("ChoiceDeck").transform);
                _clone.transform.localScale = new Vector3(1, 1, 1);
                transform.SetParent(GameObject.FindGameObjectWithTag("ChosenDeck").transform);
                UpdateNumber(1);
            }
            else if (_name == "Chosen" && _destination == "Container")
            {
                UpdateNumber(-1);
                Destroy(_clone);
                Destroy(gameObject);
            }
            else
            {
                transform.SetParent(ParentToReturnTo);
                Destroy(_clone);
            }
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(_placeholder);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var panel = GameObject.FindGameObjectWithTag("ChosenDeck");
            var d = eventData.pointerPress;
            var returnGameObject = d.transform.parent.gameObject;
            if (returnGameObject.tag.Equals("ChoiceDeck") &&
                !(panel.GetComponentsInChildren<Transform>(true).Length - 1 >= Deck.Get().RequireCard()) &&
                CheckDuplicate(d.name))
            {
                var chosen = GameObject.FindGameObjectWithTag("ChosenDeck");
                var clone = (GameObject) Instantiate(d, d.transform.position, d.transform.rotation);
                clone.transform.SetParent(chosen.transform, false);
                clone.transform.localScale = new Vector3(1, 1, 1);
                UpdateNumber(1);
            }
            else if (returnGameObject.tag.Equals("ChosenDeck"))
            {
                Destroy(d);
                UpdateNumber(-1);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var image = GameObject.FindGameObjectWithTag("DetailCard").GetComponent<Image>();
            image.sprite = CardView;
            var card = transform.GetComponent<Card>();
            var atk = GameObject.FindGameObjectWithTag("ATK");
            atk.GetComponent<Text>().text = card.Type == CardType.Unit ? card.Stats.Atk.ToString() : "";
            var hp = GameObject.FindGameObjectWithTag("HP");
            hp.GetComponent<Text>().text = card.Type == CardType.Unit ? card.Stats.Hp.ToString() : "";
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var image = GameObject.FindGameObjectWithTag("DetailCard").GetComponent<Image>();
            var atk = GameObject.FindGameObjectWithTag("ATK");
            atk.GetComponent<Text>().text = "";
            var hp = GameObject.FindGameObjectWithTag("HP");
            hp.GetComponent<Text>().text = "";
            image.sprite = null;
        }

        private bool CheckDuplicate(string obname)
        {
            var counter = 0;
            var str1 = obname.Substring(0, obname.IndexOf("(Clone)", StringComparison.Ordinal));
            var panel = GameObject.FindGameObjectWithTag("ChosenDeck");
            var allCard = panel.GetComponentsInChildren<Transform>(true);
            foreach (var card in allCard)
            {
                if (card.name.Contains(str1))
                    counter++;
                if (counter >= 2)
                    return false;
            }
            return true;
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