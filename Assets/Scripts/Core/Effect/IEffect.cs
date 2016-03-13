namespace Assets.Scripts.Core.Effect
{
    public interface IEffect
    {
        void OnAttack();
        void OnDefence(Card target);
        void OnCombat(IDamagable target);
        void OnEnter();
        void OnEnter(Card target);
        bool CanAttack();
        bool CanDefence();
        void OnEndAttack();
        void OnEndDefence();
        int GetDamageMod(IDamagable target, int damage);
        int TakeDamageMod(int damage);
        void SetParent(Card parent);
    }
}