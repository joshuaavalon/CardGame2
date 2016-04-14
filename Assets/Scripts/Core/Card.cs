using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Effect;
using Assets.Scripts.Core.Message;
using Assets.Scripts.Core.Statistics;
using Assets.Scripts.Infrastructure.EventAggregator;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Core
{
    public class Card : IEquatable<Card>, IDamagable, IHandle<TurnStartMessage>
    {
        private readonly CardStats _stats;
        public readonly IEnumerable<IEffect> Effects;
        public readonly string Id;
        public readonly string Name;
        public readonly IEnumerable<UnitType> Tags;
        public readonly TargetType Target;
        public readonly CardType Type;
        private Player _parent;
        private ZoneType _zone;
        public bool FirstTurnPlay;
        public bool HasAttack;

        public Card(string id, string name, Gui.Card card, Player parent, ZoneType zone)
        {
            _stats = new CardStats(card.Stats);
            Name = name;
            Type = card.Type;
            Id = id;
            Tags = card.Tags;
            _parent = parent;
            _zone = zone;
            Target = card.Target;
            Effects = card.GetInterfaces<IEffect>();
            FirstTurnPlay = true;
            Parent.Game.Subscribe(this);
            foreach (var effect in Effects)
            {
                effect.SetParent(this);
            }
        }

        public ZoneType Zone
        {
            get { return _zone; }
            set
            {
                if (_zone == value) return;
                _zone = value;
                NotifyZoneChange();
            }
        }

        public Player Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value) return;
                _parent = value;
                NotifyZoneChange();
            }
        }

        public void TakeDamage(int damage)
        {
            var final = Effects.Aggregate(damage,
                (current, effect) => effect.TakeDamageMod(current));
            SetStats(CardStatsType.Hp, GetStats(CardStatsType.Hp) - final);
        }

        public bool Equals(Card other)
        {
            return Id == other.Id;
        }

        public void Handle(TurnStartMessage message)
        {
            if (_zone != ZoneType.BattleField || message.Parent != Parent.Type) return;
            FirstTurnPlay = false;
            HasAttack = false;
        }

        public void OnEnter(Card target = null)
        {
            if (target == null)
                foreach (var effect in Effects)
                    effect.OnEnter();
            else
                foreach (var effect in Effects)
                    effect.OnEnter(target);
            if (Type == CardType.Event)
                RemoveSelf();
        }

        public bool CanAttack()
        {
            return Effects.Aggregate(!FirstTurnPlay && GetStats(CardStatsType.Atk) > 0
                , (current, effect) => current && effect.CanAttack());
        }

        public bool CanDefence()
        {
            return Effects.Aggregate(!HasAttack, (current, effect) => current && effect.CanDefence());
        }

        public void OnAttack()
        {
            HasAttack = true;
            foreach (var effect in Effects)
            {
                effect.OnAttack();
            }
        }

        public void Combat(IDamagable target)
        {
            foreach (var effect in Effects)
            {
                effect.OnCombat(target);
            }
            var damage = GetDamage(target);
            var player = target as Player;
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            else
            {
                var card = target as Card;
                if (card == null) return;
                card.Defence(this);
                card.TakeDamage(GetDamage(card));
                TakeDamage(card.GetDamage(this));
                card.EndDefence();
            }

            foreach (var effect in Effects)
            {
                effect.OnEndAttack();
            }
        }

        private int GetDamage(IDamagable target)
        {
            return Effects.Aggregate(_stats[CardStatsType.Atk],
                (current, effect) => effect.GetDamageMod(target, current));
        }

        private void Defence(Card card)
        {
            foreach (var effect in Effects)
            {
                effect.OnDefence(card);
            }
        }

        private void EndDefence()
        {
            foreach (var effect in Effects)
            {
                effect.OnEndDefence();
            }
        }

        public Gui.Card.Statistics GetStatistics()
        {
            return _stats.ToStatistics();
        }

        public int GetStats(CardStatsType type)
        {
            return _stats[type];
        }

        public void SetStats(CardStatsType type, int value)
        {
            if (_stats[type] == value) return;
            _stats[type] = value;
            NotifyStatsChange();
            if (type != CardStatsType.Hp || _stats[type] > 0) return;
            RemoveSelf();
        }

        private void NotifyStatsChange()
        {
            Parent.Game.Publish(new CardStatsChangeMessage(this));
        }

        private void NotifyZoneChange()
        {
            Parent.Game.Publish(new CardZoneChangeMessage(this));
        }

        private void NotifyCardDead()
        {
            Parent.Game.Publish(new CardDeadMessage(this));
        }

        private void RemoveSelf()
        {
            Parent.Remove(this);
            NotifyCardDead();
        }
    }
}