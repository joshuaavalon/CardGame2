using System.Collections.Generic;
// ReSharper disable ConvertIfStatementToNullCoalescingExpression

namespace Assets.Scripts.Core
{
    public class Deck
    {
        private static Deck _deck;

        private readonly IList<string> _list;

        private Deck()
        {
            _list = new List<string>();
        }

        public static Deck Get()
        {
            if (_deck == null)
                _deck = new Deck();
            return _deck;
        }

        public void Add(string card)
        {
            _list.Add(card);
        }

        public bool Remove(string card)
        {
            return _list.Remove(card);
        }

        public IEnumerable<string> GetList()
        {
            return _list;
        }

        public void Clear()
        {
            _list.Clear();
        }

        public int Count()
        {
            return _list.Count;
        }

        public int RequireCard()
        {
            return 5;
        }

        public int MaxOfSingle()
        {
            return 2;
        }

        public bool EnoughCard()
        {
            return Count() == RequireCard();
        }
    }
}