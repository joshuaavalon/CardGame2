using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class HoverCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private static GameObject _hover;
        private GuiMediator _guiMediator;
        [HideInInspector] public Sprite Sprite;
        [HideInInspector] public Card.Statistics Stats;
        [HideInInspector] public CardType Type;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_hover != null)
                Destroy(_hover.gameObject);
            _hover = Instantiate(_guiMediator.HoverCard);
            _hover.GetComponent<Image>().sprite = Sprite;
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
            var x = rect.anchoredPosition.x+350;
            var y = world[1][1] > Screen.height ? -150 : rect.anchoredPosition.y;
            rect.anchoredPosition = new Vector3(x, y);
            rect.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_hover != null)
                Destroy(_hover.gameObject);
        }

        private void Awake()
        {
            _guiMediator = GameObject.FindGameObjectWithTag(Tag.GuiMediator).GetComponent<GuiMediator>();
        }
    }
}