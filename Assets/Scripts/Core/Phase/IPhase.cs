using Assets.Scripts.Gui.Event;

namespace Assets.Scripts.Core.Phase
{
    public interface IPhase
    {
        /// <summary>
        ///     Move to next Phase.
        /// </summary>
        void Next();
        string GetName();
        PlayerType GetParent();
        void Handle(CardDragToZoneEventArgs args);
        void Handle(CardDragToCardEventArgs args);
        /// <summary>
        ///     Start the phase.
        /// </summary>
        void Start();
    }
}