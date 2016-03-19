using Assets.Scripts.Core.Message;
using Assets.Scripts.Gui.Event;

namespace Assets.Scripts.Core.Phase
{
    public abstract class BasePhase : IPhase
    {
        protected readonly Game Game;

        /// <summary>
        ///     Parent of the phase which can decide when to end phase.
        /// </summary>
        public readonly PlayerType Parent;

        protected BasePhase(Game game, PlayerType parent)
        {
            Game = game;
            Parent = parent;
        }

        protected abstract BasePhase NextPhase();


        /// <summary>
        ///     To be overriden by child.
        ///     Execute after PhaseStartMessage.
        /// </summary>
        protected virtual void Execute()
        {
        }

        #region IPhase

        /// <summary>
        ///     Move to next Phase.
        /// </summary>
        public void Next()
        {
            var main = this as SecondMainPhase;
            if (main != null)
                Game.Publish(new TurnEndMessage(Parent));
            Game.SetPhase(NextPhase());
        }

        /// <summary>
        ///     Get the name of phase.
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        public PlayerType GetParent()
        {
            return Parent;
        }

        /// <summary>
        ///     Start the phase.
        /// </summary>
        public void Start()
        {
            var main = this as MainPhase;
            var main2 = this as SecondMainPhase;
            if (main != null && main2 == null)
                Game.Publish(new TurnStartMessage(Parent));
            Game.Publish(new PhaseStartMessage(this));
            Execute();
        }

        public virtual void Handle(CardDragToZoneEventArgs args)
        {
        }

        public virtual void Handle(CardDragToCardEventArgs args)
        {
        }

        #endregion
    }
}