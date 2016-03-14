using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.DeckEdit
{
    public class HoverCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private DeckHandler _deckHandler;
        private static GameObject _hover;

        [HideInInspector] public int Attack;
        [HideInInspector] public int Hp;
        [HideInInspector] public Sprite Image;
        [HideInInspector] public bool IsThumb;
        [HideInInspector] public CardType Type;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_hover != null)
                Destroy(_hover.gameObject);
            _hover = Instantiate(_deckHandler.HoverCardPrefab);
            _hover.GetComponent<Image>().sprite = Image;
            _hover.transform.SetParent(transform, false);

            var vector = Camera.main.WorldToScreenPoint(transform.position);
            float x;
            float y;
            if (IsThumb)
            {
                x = transform.localPosition.x + 700;
                y = transform.localPosition.y + (vector.y < -220 ? 875 : 450);
            }
            else
            {
                x = transform.localPosition.x + (vector.x < 250 ? 350 : 50);
                y = transform.localPosition.y + (vector.y < 300 ? 850 : 400);
            }
            _hover.transform.localPosition = new Vector3(x, y, 0);

            var texts = _hover.GetComponentsInChildren<Text>();
            foreach (var text in texts)
            {
                if (text.tag == Tag.ATK)
                    text.text = Type == CardType.Unit ? Attack.ToString() : "";
                if (text.tag == Tag.HP)
                    text.text = Type == CardType.Unit ? Hp.ToString() : "";
            }
            _hover.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform, false);

        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (_hover != null)
                Destroy(_hover.gameObject);
        }

        private void Start()
        {
            _deckHandler = GameObject.FindGameObjectWithTag(Tag.DeckHandler).GetComponent<DeckHandler>();
        }
    }
}