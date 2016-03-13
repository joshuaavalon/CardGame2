using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Message;
using Assets.Scripts.Core.Statistics;

// ReSharper disable ConvertPropertyToExpressionBody

namespace Assets.Scripts.Core
{
    public class Player : IDamagable
    {
        private readonly IList<Card> _battlefield;
        private readonly IList<Card> _hand;
        private readonly IList<string> _scrap;
        private readonly PlayerStats _stats;
        public readonly Game Game;
        public readonly PlayerType Type;

        public Player(Game game, PlayerType type)
        {
            Game = game;
            Type = type;
            _stats = new PlayerStats();
            _battlefield = new List<Card>();
            _hand = new List<Card>();
            _scrap = new List<string>();
        }

        public IEnumerable<Card> Battlefield
        {
            get { return _battlefield; }
        }

        public IEnumerable<Card> Hand
        {
            get { return _hand; }
        }

        public IEnumerable<string> Scrap
        {
            get { return _scrap; }
        }

        public void TakeDamage(int damage)
        {
            SetStats(PlayerStatsType.Hp, GetStats(PlayerStatsType.Hp) - damage);
        }

        public void Add(ZoneType zone, Card card)
        {
            switch (zone)
            {
                case ZoneType.Hand:
                    _hand.Add(card);
                    break;
                case ZoneType.BattleField:
                    _battlefield.Add(card);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(zone.ToString(), zone, null);
            }
        }

        public void RestoreResource()
        {
            _stats[PlayerStatsType.Metal] = _stats[PlayerStatsType.MaxMetal];
            _stats[PlayerStatsType.Crystal] = _stats[PlayerStatsType.MaxCrystal];
            _stats[PlayerStatsType.Deuterium] = _stats[PlayerStatsType.MaxDeuterium];
            NotifyStatsChange();
        }

        public Card GetCardById(string id)
        {
            foreach (var card in _battlefield.Where(card => card.Id == id))
                return card;
            return _hand.FirstOrDefault(card => card.Id == id);
        }

        private bool EnoughCost(Card card)
        {
            return GetStats(PlayerStatsType.Metal) >= card.GetStats(CardStatsType.Metal) &&
                   GetStats(PlayerStatsType.Crystal) >= card.GetStats(CardStatsType.Crystal) &&
                   GetStats(PlayerStatsType.Deuterium) >= card.GetStats(CardStatsType.Deuterium);
        }

        public bool Play(Card card, Card target = null)
        {
            if (!EnoughCost(card))
            {
                Game.GuiMediator.ShowMessage("You don't have enough resource(s)");
                return false;
            }
            _stats[PlayerStatsType.Metal] -= card.GetStats(CardStatsType.Metal);
            _stats[PlayerStatsType.Crystal] -= card.GetStats(CardStatsType.Crystal);
            _stats[PlayerStatsType.Deuterium] -= card.GetStats(CardStatsType.Deuterium);
            _hand.Remove(card);
            NotifyStatsChange();
            switch (card.Type)
            {
                case CardType.Event:
                case CardType.Unit:
                    _battlefield.Add(card);
                    card.Zone = ZoneType.BattleField;
                    card.OnEnter(target);
                    return true;
                default:
                    return false;
            }
        }

        public IEnumerable<Card> GetAttackUnit()
        {
            return _battlefield.Where(card => card.CanAttack());
        }

        public IEnumerable<Card> GetDefenceUnit()
        {
            return _battlefield.Where(card => card.CanDefence());
        }

        public void Remove(string id)
        {
            Remove(GetCardById(id));
        }

        public void Remove(Card card)
        {
            if (card.Zone == ZoneType.BattleField)
                _battlefield.Remove(card);
            else
                _hand.Remove(card);
            _scrap.Add(card.Name);
        }

        public IEnumerable<string> GetCardOnBattleField()
        {
            return _battlefield.Select(card => card.Id);
        }


        public int GetStats(PlayerStatsType type)
        {
            return _stats[type];
        }

        public void SetStats(PlayerStatsType type, int value)
        {
            if (_stats[type] == value) return;
            _stats[type] = value;
            NotifyStatsChange();
        }

        private void NotifyStatsChange()
        {
            Game.Publish(new PlayerStatsChangeMessage(this));
        }

        public PlayerStats GePlayerStats()
        {
            return new PlayerStats(_stats);
        }
    }
}