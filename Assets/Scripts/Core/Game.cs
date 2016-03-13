using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Message;
using Assets.Scripts.Core.Phase;
using Assets.Scripts.Core.Statistics;
using Assets.Scripts.Gui;
using Assets.Scripts.Gui.Event;
using Assets.Scripts.Infrastructure.EventAggregator;
using Assets.Scripts.Infrastructure.IdFactory;
using Assets.Scripts.Infrastructure.Logging;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation

namespace Assets.Scripts.Core
{
    public class Game : MonoBehaviour, IHandle<PhaseStartMessage>, IHandle<PlayerStatsChangeMessage>,
        IHandle<CardZoneChangeMessage>, IHandle<CardDeadMessage>, IHandle<CardStatsChangeMessage>
    {
        private const int MaximumResource = 10;
        public static bool End;
        private Battle _battle;
        private IList<string> _deck;
        private EventAggregator _eventAggregator;
        private PlayerType _first;
        private IIdFactory _idFactory;
        private IPhase _phase;
        private Dictionary<PlayerType, Player> _players;
        public GameObject EndGame;
        public Text EndText;
        public GuiMediator GuiMediator;
        public Button QuitButton;

        #region Battle

        private class Battle
        {
            private readonly string[] _attacker;
            private readonly Dictionary<string, string> _battle;
            private readonly Game _game;
            private readonly PlayerType _player;

            internal Battle(Game game, PlayerType player, string[] attacker)
            {
                _attacker = attacker;
                _game = game;
                _player = player;
                _battle = new Dictionary<string, string>();
            }

            public void AddBattle(string defender, string attacker)
            {
                if (!_attacker.Contains(attacker)) return;
                if (_battle.ContainsKey(defender) || _battle.ContainsValue(attacker)) return;
                _battle.Add(defender, attacker);
                _game.SetDraggable(defender, false);
                _game.SetColor(attacker, ColorType.Normal);
                var atk = _game.GetCardById(attacker);
                var def = _game.GetCardById(defender);
                atk.Combat(def);
            }

            public IEnumerable<string> GetAttacker()
            {
                return _attacker;
            }

            public void Resolve()
            {
                var notDef = _attacker.Where(attacker => !_battle.ContainsValue(attacker)).ToList();
                var opponent = _game.GetPlayer(_player.Opposite());
                foreach (var card in notDef.Select(attacker => _game.GetCardById(attacker)))
                {
                    card.Combat(opponent);
                }
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Log.Verbose("Game Awake", "Unity");
            // TODO: Random Start
            _eventAggregator = new EventAggregator();
            _first = PhotonNetwork.isMasterClient ? PlayerType.Player : PlayerType.Opponent;
            _idFactory = new CardIdFactory();
            _players = new Dictionary<PlayerType, Player>();
            foreach (var type in Extension.GetValues<PlayerType>())
                _players.Add(type, new Player(this, type));
            _battle = null;
            _deck = new List<string>(Deck.Get().GetList());
            _deck.Shuffle();
            Subscribe(this);


            GuiMediator.OnButtonClick += OnButtonClick;
            GuiMediator.SetButtonClickable(ButtonType.NextPhaseButton, PhotonNetwork.isMasterClient);
            GuiMediator.OnCardDragToZone += OnCardDragToZone;
            GuiMediator.OnCardDragToCard += OnCardDragToCard;
            QuitButton.onClick.AddListener(() => { PhotonNetwork.LeaveRoom(); });
            End = false;
        }

        private void Start()
        {
            Log.Verbose("Start Game", "Unity");
            foreach (var player in _players)
                Publish(new PlayerStatsChangeMessage(player.Value));
            SetPhase(new MainPhase(this, _first));
            if (_first == PlayerType.Player)
            {
                DrawCard(PlayerType.Player);
                DrawCard(PlayerType.Player);
                DrawCard(PlayerType.Player);
            }
            else
            {
                DrawCard(PlayerType.Player);
                DrawCard(PlayerType.Player);
                DrawCard(PlayerType.Player);
                DrawCard(PlayerType.Player);
            }
        }

        #endregion

        #region Event

        public void Publish(GameMessage message)
        {
            _eventAggregator.Publish(message);
        }

        public void Subscribe(object subscriber)
        {
            _eventAggregator.Subscribe(subscriber);
        }

        #endregion

        #region Gui

        private void OnButtonClick(object sender, ButtonClickEventArgs args)
        {
            Log.Verbose("OnButtonClick", "GUI");
            var type = args.Type;
            if (type != ButtonType.NextPhaseButton) return;
            GetComponent<PhotonView>().RPC("NextPhase", PhotonTargets.AllViaServer);
        }

        private void OnCardDragToZone(object sender, CardDragToZoneEventArgs args)
        {
            Log.Verbose("OnCardDragToZone", "GUI");
            _phase.Handle(args);
        }

        private void OnCardDragToCard(object sender, CardDragToCardEventArgs args)
        {
            Log.Verbose("OnCardDragToCard", "GUI");
            _phase.Handle(args);
        }

        public void SetFrontAndDrag(string id, PlayerType owner, ZoneType destination)
        {
            var front = !(owner == PlayerType.Opponent && destination == ZoneType.Hand);
            GuiMediator.SetCardIsFront(id, front);
            var drag = owner == PlayerType.Player && destination == ZoneType.Hand;
            SetDraggable(id, drag);
        }

        private void SetColor(string id, ColorType colorType)
        {
            GuiMediator.SelectColor(id, colorType);
        }

        private void SetDraggable(string id, bool drag)
        {
            GuiMediator.SetDraggable(id, drag);
        }

        #endregion

        #region Game

        public void SetPhase(IPhase phase)
        {
            Log.Verbose("Set Phase: " + phase.GetName() + "(" + phase.GetParent() + ")", "Game");
            _phase = phase;
            _phase.Start();
        }

        public string GetCardId(PlayerType type)
        {
            return _idFactory.GetId(_first == type ? CardIdFactory.FirstPlayer : CardIdFactory.SecondPlayer);
        }

        public Player GetPlayer(PlayerType type)
        {
            return _players[type];
        }

        private Player GetPlayerByCardId(string id)
        {
            return GetPlayer(_idFactory.GetType(id) == CardIdFactory.FirstPlayer ? _first : _first.Opposite());
        }

        public Card GetCardById(string id)
        {
            return GetPlayerByCardId(id).GetCardById(id);
        }

        public void TryPlay(string id, string target = null)
        {
            var player = GetPlayerByCardId(id);
            var card = player.GetCardById(id);
            var targetCard = target == null ? null : GetPlayerByCardId(target).GetCardById(target);
            if (card.Target != TargetType.Both && targetCard == null)
            {
                GuiMediator.ShowMessage("You have to target a unit");
                return;
            }
            if (targetCard != null && targetCard.Zone != ZoneType.BattleField)
            {
                GuiMediator.ShowMessage("You can only target unit on battlefield");
                return;
            }
            if (card.Target == TargetType.AllyUnit && targetCard.Parent.Type != PlayerType.Player)
            {
                GuiMediator.ShowMessage("You can only target ally unit");
                return;
            }
            if (card.Target == TargetType.EnemyUnit && targetCard.Parent.Type != PlayerType.Opponent)
            {
                GuiMediator.ShowMessage("You can only target enemy unit");
                return;
            }
            if (!player.Play(card, targetCard)) return;
            if (targetCard == null)
                GetComponent<PhotonView>().RPC("RpcPlayCard", PhotonTargets.Others, id);
            else
                GetComponent<PhotonView>().RPC("RpcPlayCard", PhotonTargets.Others, id, targetCard.Id);
        }

        public void EnabledResourcePanel()
        {
            var player = _players[PlayerType.Player];
            var enableMetal = player.GetStats(PlayerStatsType.MaxMetal) < MaximumResource;
            var enableCrystal = player.GetStats(PlayerStatsType.MaxCrystal) < MaximumResource;
            var enableDeuterium = player.GetStats(PlayerStatsType.MaxDeuterium) < MaximumResource;
            GuiMediator.EnableResourcePanel(rType => { AddResource(player.Type, rType, 1, true); },
                enableMetal, enableCrystal, enableDeuterium);
        }

        public void AddResource(PlayerType pType, ResourceType rType, int value, bool restore = false)
        {
            var bytePlayerType = (byte) pType;
            var byteOpponentType = (byte) pType.Opposite();
            var byteResourceType = (byte) rType;

            RpcAddResource(bytePlayerType, byteResourceType, value, restore);
            GetComponent<PhotonView>()
                .RPC("RpcAddResource", PhotonTargets.Others, byteOpponentType, byteResourceType, value, restore);
        }

        public void DrawCard(PlayerType type)
        {
            if (_deck.Count <= 0) return;
            var cardName = _deck[0];
            _deck.RemoveAt(0);
            var id = GetCardId(type);
            CreateCard(cardName, id, type, ZoneType.Hand);
        }

        public void CreateCard(string cardName, string id, PlayerType owner, ZoneType destination)
        {
            var bytePlayerType = (byte) owner;
            var byteOpponentType = (byte) owner.Opposite();
            var byteDestinationType = (byte) destination;
            RpcCreateCard(cardName, id, bytePlayerType, byteDestinationType);
            GetComponent<PhotonView>()
                .RPC("RpcCreateCard", PhotonTargets.Others, cardName, id, byteOpponentType, byteDestinationType);
        }

        public void CreateBattle(PlayerType player, string[] attacker)
        {
            var bytePlayerType = (byte) player;
            var byteOpponentType = (byte) player.Opposite();
            RpcCreateBattle(bytePlayerType, attacker);
            GetComponent<PhotonView>().RPC("RpcCreateBattle", PhotonTargets.Others, byteOpponentType, attacker);
            if (attacker == null)
                GetComponent<PhotonView>().RPC("NextPhase", PhotonTargets.AllViaServer);
        }

        public void AddBattle(string defender, string attacker)
        {
            Log.Verbose(attacker + ":" + defender, "Battle");
            GetComponent<PhotonView>().RPC("RpcAddBattle", PhotonTargets.All, defender, attacker);
        }

        public void ResolveBattle()
        {
            if (_battle == null) return;
            _battle.Resolve();
            _battle = null;
            foreach (var id in GetPlayer(PlayerType.Player).GetCardOnBattleField())
                SetColor(id, ColorType.Normal);
            foreach (var id in GetPlayer(PlayerType.Opponent).GetCardOnBattleField())
                SetColor(id, ColorType.Normal);
        }

        #endregion

        #region Handle

        public void Handle(PhaseStartMessage message)
        {
            var phase = message.Phase;
            GuiMediator.SetButtonClickable(ButtonType.NextPhaseButton, phase.GetParent() == PlayerType.Player);
            GuiMediator.SetPhaseText(phase.GetName());
        }

        public void Handle(PlayerStatsChangeMessage message)
        {
            var player = message.Player;
            GuiMediator.UpdatePlayerStats(player.Type, player.GePlayerStats());
            if (player.GetStats(PlayerStatsType.Hp) > 0) return;
            End = true;
            EndText.text = player.Type == PlayerType.Player ? "You Lose" : "You Win";
            EndGame.SetActive(true);
        }

        public void Handle(CardZoneChangeMessage message)
        {
            var card = message.Card;
            GuiMediator.MoveCard(card.Id, card.Parent.Type, card.Zone);
            SetFrontAndDrag(card.Id, card.Parent.Type, card.Zone);
        }

        public void Handle(CardDeadMessage message)
        {
            GuiMediator.DestoryCard(message.Card.Id);
        }

        public void Handle(CardStatsChangeMessage message)
        {
            var card = message.Card;
            GuiMediator.UpdateCardStats(card.Id, card.GetStatistics());
        }

        #endregion

        #region Photon

        [PunRPC]
        public void NextPhase()
        {
            Log.Verbose("NextPhase", "RPC");
            Publish(new PhaseEndMessage(_phase));
            _phase.Next();
        }

        [PunRPC]
        private void RpcAddResource(byte bytePlayerType, byte byteResourceType, int value, bool restore = false)
        {
            Log.Verbose("AddResource", "RPC");
            var pType = (PlayerType) bytePlayerType;
            var rType = (ResourceType) byteResourceType;
            var player = _players[pType];
            switch (rType)
            {
                case ResourceType.Metal:
                    player.SetStats(PlayerStatsType.MaxMetal, player.GetStats(PlayerStatsType.MaxMetal) + value);
                    break;
                case ResourceType.Crystal:
                    player.SetStats(PlayerStatsType.MaxCrystal, player.GetStats(PlayerStatsType.MaxCrystal) + value);
                    break;
                case ResourceType.Deuterium:
                    player.SetStats(PlayerStatsType.MaxDeuterium, player.GetStats(PlayerStatsType.MaxDeuterium) + value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (restore)
                player.RestoreResource();
        }

        [PunRPC]
        private void RpcCreateCard(string cardName, string id, byte byteOwner, byte byteDestination)
        {
            var owner = (PlayerType) byteOwner;
            var destination = (ZoneType) byteDestination;

            var player = GetPlayer(owner);
            var cardComponent = GuiMediator.CreateCard(cardName, id, owner, destination);
            var card = new Card(id, cardName, cardComponent, player, destination);
            player.Add(destination, card);
            SetFrontAndDrag(id, owner, destination);
        }

        [PunRPC]
        private void RpcPlayCard(string id)
        {
            var player = GetPlayerByCardId(id);
            var card = player.GetCardById(id);
            player.Play(card);
        }

        [PunRPC]
        private void RpcPlayCard(string id, string target)
        {
            var player = GetPlayerByCardId(id);
            var card = player.GetCardById(id);
            var player2 = GetPlayerByCardId(target);
            var card2 = player2.GetCardById(target);
            player.Play(card, card2);
        }

        [PunRPC]
        private void RpcCreateBattle(byte bytePlayerType, string[] attacker)
        {
            var player = (PlayerType) bytePlayerType;
            if (attacker == null) return;
            _battle = new Battle(this, player, attacker);
            foreach (var id in _battle.GetAttacker())
            {
                SetColor(id, ColorType.Selected);
                GetCardById(id).OnAttack();
            }
        }

        [PunRPC]
        private void RpcAddBattle(string defender, string attacker)
        {
            if (_battle == null) return;
            _battle.AddBattle(defender, attacker);
        }

        #endregion
    }
}