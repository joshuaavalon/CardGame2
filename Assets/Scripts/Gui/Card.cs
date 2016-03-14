using System;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Statistics;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation

namespace Assets.Scripts.Gui
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _cardImage;
        private GuiMediator _guiMediator;
        private GameObject _hover;

        private string _id = "";
        private bool _isFront;
        public Sprite Image;
        public Sprite Thumbnail;
        public Statistics Stats; // For grouping in inspector
        public UnitType[] Tags;
        public TargetType Target;
        public CardType Type;

        public bool IsFront
        {
            set
            {
                if (_isFront == value) return;
                _isFront = value;
                _cardImage.sprite = _isFront ? Image : _guiMediator.CardBack;
            }
        }

        public PlayerType Parent { get; set; }
        public ZoneType Zone { get; set; }

        /// <summary>
        ///     Card id.
        /// </summary>
        public string Id
        {
            get { return _id; }
            set
            {
                if (value != null && _id == "")
                    _id = value;
            }
        }

        /// <summary>
        ///     Change the card view image when mouse enter a card.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isFront) return;

            _hover = Instantiate(_guiMediator.HoverCard);
            _hover.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
            _hover.transform.SetParent(transform, false);

            var texts = _hover.GetComponentsInChildren<Text>();
            foreach (var text in texts)
            {
                if (text.tag == Tag.ATK)
                    text.text = Type == CardType.Unit ? Stats.Atk.ToString() : "";
                if (text.tag == Tag.HP)
                    text.text = Type == CardType.Unit ? Stats.Hp.ToString() : "";
            }
            var world = new Vector3[4];
            var rect = _hover.GetComponent<RectTransform>();
            rect.GetWorldCorners(world);
            var x = rect.anchoredPosition.x;
            var y = world[1][1] > Screen.height ? -150 : rect.anchoredPosition.y;
            rect.anchoredPosition = new Vector3(x, y);
            rect.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (_hover != null)
                Destroy(_hover.gameObject);
        }

        /// <summary>
        ///     Use this for initialization
        /// </summary>
        private void Awake()
        {
            _guiMediator = GameObject.FindGameObjectWithTag(Tag.GuiMediator).GetComponent<GuiMediator>();
            _cardImage = GetComponent<Image>();
        }

        public void SetStats(CardStatsType type, int value)
        {
            switch (type)
            {
                case CardStatsType.Hp:
                    Stats.Hp = value;
                    break;
                case CardStatsType.Atk:
                    Stats.Atk = value;
                    break;
                case CardStatsType.Metal:
                    Stats.Metal = value;
                    break;
                case CardStatsType.Crystal:
                    Stats.Crystal = value;
                    break;
                case CardStatsType.Deuterium:
                    Stats.Deuterium = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(type.GetType().Name, type, null);
            }
        }

        public int GetStats(CardStatsType type)
        {
            switch (type)
            {
                case CardStatsType.Hp:
                    return Stats.Hp;
                case CardStatsType.Atk:
                    return Stats.Atk;
                case CardStatsType.Metal:
                    return Stats.Metal;
                case CardStatsType.Crystal:
                    return Stats.Crystal;
                case CardStatsType.Deuterium:
                    return Stats.Deuterium;
                default:
                    throw new ArgumentOutOfRangeException(type.GetType().Name, type, null);
            }
        }

        /// <summary>
        ///     For initial value and inspector view only.
        /// </summary>
        [Serializable]
        public class Statistics
        {
            public int Hp, Atk, Metal, Crystal, Deuterium;
        }
    }
}