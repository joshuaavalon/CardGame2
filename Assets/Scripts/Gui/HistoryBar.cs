using Assets.Scripts.Core;
using Assets.Scripts.Core.Message;
using Assets.Scripts.Infrastructure.EventAggregator;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class HistoryBar: MonoBehaviour, IHandle<CardPlayMessage>
    {
        public Transform Parent;
        public GameObject ScrollItemPrefab;
        private GuiMediator _guiMediator;

        private void AddHistory(Card card)
        {
            var item = Instantiate(ScrollItemPrefab);
            var hoverCard = item.GetComponentInChildren<HoverCard>();
            var image = item.GetComponentsInChildren<Image>()[1];
            image.sprite = card.Thumbnail;
            hoverCard.Sprite = card.Image;
            hoverCard.Stats = new Card.Statistics(card.Stats);
            hoverCard.Type = card.Type;
            item.transform.SetParent(Parent);
            item.transform.localScale=Vector3.one;
            item.transform.SetAsFirstSibling();
        }

        public void Handle(CardPlayMessage message)
        {
            var card = _guiMediator.GetCardById(message.Card.Id).GetComponent<Card>();
            AddHistory(card);
        }

        private void Awake()
        {
            _guiMediator = GameObject.FindGameObjectWithTag(Tag.GuiMediator).GetComponent<GuiMediator>();
            var game = GameObject.FindGameObjectWithTag(Tag.GameController).GetComponent<Game>();
            game.Subscribe(this);
        }
    }
}
