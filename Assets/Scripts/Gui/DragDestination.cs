using System;
using Assets.Scripts.Core;
using Assets.Scripts.Gui.Event;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Gui
{
    public class DragDestination : MonoBehaviour, IEndDragHandler
    {
        public void OnEndDrag(PointerEventData eventData)
        {
            var id = GetComponent<Card>().Id;
            var currentTag = eventData.pointerEnter.tag;
            var card = eventData.pointerEnter.GetComponent<Card>();
            if (currentTag != Tag.Hand && currentTag != Tag.Battlefield && card == null) return;
            var parentTag = eventData.pointerEnter.transform.parent.tag;
            if (card != null)
                OnCardDragToCard(this, new CardDragToCardEventArgs(id, card.Id));
            else
            {
                var zoneType = currentTag == Tag.Hand ? ZoneType.Hand : ZoneType.BattleField;
                var ownerType = parentTag == Tag.Player ? PlayerType.Player : PlayerType.Opponent;
                OnCardDragToZone(this, new CardDragToZoneEventArgs(id, zoneType, ownerType));
            }
        }

        public event EventHandler<CardDragToCardEventArgs> OnCardDragToCard = (sender, args) => { };
        public event EventHandler<CardDragToZoneEventArgs> OnCardDragToZone = (sender, args) => { };
    }
}