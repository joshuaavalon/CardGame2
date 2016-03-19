using System;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Statistics;
using Assets.Scripts.Gui.Controller;
using Assets.Scripts.Gui.Event;
using Assets.Scripts.Infrastructure.Logging;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UseStringInterpolation

namespace Assets.Scripts.Gui
{
    /// <summary>
    ///     Use to interact between UI elements and game core.
    /// </summary>
    public class GuiMediator : MonoBehaviour
    {
        // You should store the card id and the referenced gameobject
        private Dictionary<string, GameObject> _idDictionary;
        private bool _oneSelected;
        public GameObject Canvas;
        public Sprite CardBack;
        public GameObject GameMessage;
        public Button NextButton;
        public PlayerController Opponent;
        public Text PhaseText;
        public PlayerController Player;
        public ResourcePanelController ResourcePanelController;
        public GameObject HoverCard;

        /// <summary>
        ///     This is call when a button is clicked.
        /// </summary>
        /// <remarks>Do not remove the empty function.</remarks>
        public event EventHandler<ButtonClickEventArgs> OnButtonClick = (sender, args) => { };

        /// <summary>
        ///     This is call when a card drag to another card.
        /// </summary>
        /// <remarks>Do not remove the empty function.</remarks>
        public event EventHandler<CardDragToCardEventArgs> OnCardDragToCard = (sender, args) => { };

        /// <summary>
        ///     This is call when a card drag to another zone.
        /// </summary>
        /// <remarks>Do not remove the empty function.</remarks>
        public event EventHandler<CardDragToZoneEventArgs> OnCardDragToZone = (sender, args) => { };

        /// <summary>
        ///     Use this for initialization
        /// </summary>
        private void Awake()
        {
            _idDictionary = new Dictionary<string, GameObject>();
            NextButton.onClick.AddListener(
                () => { OnButtonClick(this, new ButtonClickEventArgs(ButtonType.NextPhaseButton)); });
            _oneSelected = false;
        }

        /// <summary>
        ///     Update player's statistics.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stats"></param>
        public void UpdatePlayerStats(PlayerType type, PlayerStats stats)
        {
            var target = type == PlayerType.Player ? Player : Opponent;
            target.Stats.SetText(PlayerStatsType.Hp,stats.Get(PlayerStatsType.Hp),
                    stats.Get(PlayerStatsType.MaxHp));
            target.Stats.SetText(PlayerStatsType.Metal, stats.Get(PlayerStatsType.Metal),
                    stats.Get(PlayerStatsType.MaxMetal));
            target.Stats.SetText(PlayerStatsType.Crystal, stats.Get(PlayerStatsType.Crystal),
                    stats.Get(PlayerStatsType.MaxCrystal));
            target.Stats.SetText(PlayerStatsType.Deuterium, stats.Get(PlayerStatsType.Deuterium),
                    stats.Get(PlayerStatsType.MaxDeuterium));
        }

        private string ColorString(string type, int current, int max)
        {
            var startTag = current == max ? "" : current == 0 ? "<Color=red>" : "<Color=yellow>";
            var endTag = current == max ? "" : "</color>";
            return string.Format("{0}: {3}{1}{4} / {2}", type, current, max, startTag, endTag);
        }

        /// <summary>
        ///     Set a button if it is clickable.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="clickable"></param>
        public void SetButtonClickable(ButtonType type, bool clickable)
        {
            if (type != ButtonType.NextPhaseButton) return;
            NextButton.interactable = clickable;
        }

        /// <summary>
        ///     Create a card (GameObject) and return its Card component.
        ///     Assign the card with given id.
        ///     The card created should place at given zone.
        /// </summary>
        /// <param name="cardName">Name of the card</param>
        /// <param name="id"></param>
        /// <param name="owner">Owner of the card</param>
        /// <param name="destination"></param>
        /// <returns>Card component</returns>
        public Card CreateCard(string cardName, string id, PlayerType owner, ZoneType destination)
        {
            var ownerController = owner == PlayerType.Player ? Player : Opponent;
            var card = Instantiate(Resources.Load(cardName)) as GameObject;
            if (card == null)
                return null;
            card.GetComponent<Card>().Id = id;
            _idDictionary.Add(id, card);
            if (destination == ZoneType.Hand)
                ownerController.MoveToHand(card);
            else
                ownerController.MoveToBattlefield(card);
            return card.GetComponent<Card>();
        }

        /// <summary>
        ///     Move a card to different area
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <param name="destination"></param>
        public void MoveCard(string id, PlayerType owner, ZoneType destination)
        {
            var card = _idDictionary[id];
            var ownerController = owner == PlayerType.Player ? Player : Opponent;
            if (destination == ZoneType.Hand)
                ownerController.MoveToHand(card);
            else
                ownerController.MoveToBattlefield(card);
        }

        /// <summary>
        ///     Set the text of Phase Text
        /// </summary>
        /// <param name="text"></param>
        public void SetPhaseText(string text)
        {
            PhaseText.text = text;
        }

        /// <summary>
        ///     Set the card IsFront property.
        /// </summary>
        /// <param name="id">Card id.</param>
        /// <param name="isFront"></param>
        public void SetCardIsFront(string id, bool isFront)
        {
            var card = _idDictionary[id];
            card.GetComponent<Card>().IsFront = isFront;
        }

        /// <summary>
        ///     Enable ResourcePanel and call onClose after a button is clicked.
        ///     The bools is to decide whether the button is clickable or not.
        /// </summary>
        /// <param name="onClose"></param>
        /// <param name="metalEnable"></param>
        /// <param name="crystalEnable"></param>
        /// <param name="deuteriumEnable"></param>
        public void EnableResourcePanel(Action<ResourceType> onClose, bool metalEnable,
            bool crystalEnable, bool deuteriumEnable)
        {
            ResourcePanelController.gameObject.SetActive(true);
            ResourcePanelController.MetalButton.interactable = metalEnable;
            ResourcePanelController.CrystalButton.interactable = crystalEnable;
            ResourcePanelController.DeuteriumButton.interactable = deuteriumEnable;
            ClearListener();
            ResourcePanelController.MetalButton.onClick.AddListener(() =>
            {
                onClose(ResourceType.Metal);
                OnButtonClick(this, new ButtonClickEventArgs(ButtonType.Undefined));
                ResourcePanelController.gameObject.SetActive(false);
            });
            ResourcePanelController.CrystalButton.onClick.AddListener(() =>
            {
                onClose(ResourceType.Crystal);
                OnButtonClick(this, new ButtonClickEventArgs(ButtonType.Undefined));
                ResourcePanelController.gameObject.SetActive(false);
            });
            ResourcePanelController.DeuteriumButton.onClick.AddListener(() =>
            {
                onClose(ResourceType.Deuterium);
                OnButtonClick(this, new ButtonClickEventArgs(ButtonType.Undefined));
                ResourcePanelController.gameObject.SetActive(false);
            });
        }


        private void ClearListener()
        {
            ResourcePanelController.MetalButton.onClick.RemoveAllListeners();
            ResourcePanelController.CrystalButton.onClick.RemoveAllListeners();
            ResourcePanelController.DeuteriumButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        ///     Enable a selection of card and call onClose after selection.
        ///     Other than card id, Keyword.TargetPlayer and Keyword.TargetOpponent should be allowed.
        /// </summary>
        /// <param name="onClose"></param>
        /// <param name="idList">ID of the cards which is able to select.</param>
        /// <param name="allowMultiple">Allow multiple selections.</param>
        /// <param name="allowCancel">Allow to cacel the selection</param>
        public void EnableSelection(Action<string[]> onClose, string[] idList, bool allowMultiple,
            bool allowCancel)
        {
            var selected = new HashSet<string>();
            foreach (var t in idList)
            {
                AddButton(t, selected, allowMultiple, allowCancel);
            }
            NextButton.onClick.AddListener(() =>
            {
                var stringArray = new string[selected.Count];
                selected.CopyTo(stringArray);
                onClose(stringArray.Length == 0 ? null : stringArray);
                foreach (var t in idList)
                {
                    var butt = _idDictionary[t].GetComponent<Button>();
                    SelectColor(t, ColorType.Normal);
                    if (butt != null)
                        Destroy(butt);
                }
                NextButton.onClick.RemoveAllListeners();
                NextButton.onClick.AddListener(
                    () => { OnButtonClick(this, new ButtonClickEventArgs(ButtonType.NextPhaseButton)); });
            });
        }

        private void AddButton(string t, HashSet<string> selected, bool allowMultiple, bool allowCancel)
        {
            var isClicked = false;
            var card = _idDictionary[t];
            var butt = card.GetComponent<Button>();
            if (butt != null)
                Destroy(butt);
            var button = card.AddComponent<Button>();
            button.interactable = true;
            button.onClick.AddListener(() =>
            {
                if (!allowMultiple)
                {
                    if (!isClicked && _oneSelected) return;
                }
                if (!isClicked || !allowCancel)
                {
                    isClicked = true;
                    _oneSelected = true;
                    SelectColor(t, ColorType.Selected);
                    selected.Add(t);
                }
                else
                {
                    isClicked = false;
                    _oneSelected = false;
                    SelectColor(t, ColorType.Normal);
                    selected.Remove(t);
                }
            });
        }


        /// <summary>
        ///     Set a card to be draggable or not.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="draggable"></param>
        public void SetDraggable(string id, bool draggable)
        {
            Log.Verbose("SetDraggable"+id+":"+draggable,"GUI");
            var card = _idDictionary[id];
            var drag = card.GetComponent<Draggable>();
            var dragDest = card.GetComponent<DragDestination>();
            if (draggable && drag == null)
            {
                card.AddComponent<Draggable>();
                var dest = card.AddComponent<DragDestination>();
                dest.OnCardDragToCard += OnCardDragToCard;
                dest.OnCardDragToZone += OnCardDragToZone;
            }
            if (draggable || drag == null || dragDest == null) return;
            Destroy(dragDest);
            Destroy(drag);
        }

        /// <summary>
        ///     Remove the card.
        /// </summary>
        /// <param name="id"></param>
        public void DestoryCard(string id)
        {
            foreach (var temp in _idDictionary)
            {
                Log.Verbose(temp.Key + ":" + temp.Value);
            }
            var target = _idDictionary[id];
            Destroy(target);
            _idDictionary.Remove(id);
        }

        /// <summary>
        ///     Set the color of the color
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colorType"></param>
        public void SelectColor(string id, ColorType colorType)
        {
            var card = _idDictionary[id];
            /*
            var butt = card.GetComponent<Button>();
            if (butt != null) { }
            else
                butt = card.AddComponent<Button>();*/
            switch (colorType)
            {
                case ColorType.Normal:
                    card.GetComponent<Image>().color = Color.white;
                    break;
                case ColorType.Selected:
                    card.GetComponent<Image>().color = new Color(0F, 0.8F, 0F, 0.5F);
                    break;
                case ColorType.Targetable:
                    card.GetComponent<Image>().color = Color.yellow;
                    break;
            }
        }

        /// <summary>
        ///     Update Card Statistics.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stats"></param>
        public void UpdateCardStats(string id, Card.Statistics stats)
        {
            _idDictionary[id].GetComponent<Card>().Stats = stats;
        }

        public void ShowMessage(string message, float time = 2f)
        {
            var gameMessage = Instantiate(GameMessage);
            gameMessage.transform.SetParent(Canvas.transform, false);
            var component = gameMessage.GetComponent<Message>();
            component.Show(message, time);
        }
    }
}