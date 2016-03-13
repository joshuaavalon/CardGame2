using UnityEngine;

namespace Assets.Scripts.Core.Effect
{
    public abstract class BaseEffect : MonoBehaviour, IEffect
    {
        protected Card Parent;

        public virtual void OnCombat(IDamagable target)
        {
        }

        public void OnAttack()
        {
        }

        public virtual void OnDefence(Card target)
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnEnter(Card target)
        {
            OnEnter();
        }

        public virtual bool CanAttack()
        {
            return true;
        }

        public virtual bool CanDefence()
        {
            return true;
        }

        public virtual void OnEndAttack()
        {
        }

        public virtual void OnEndDefence()
        {
        }

        public virtual int GetDamageMod(IDamagable target, int damage)
        {
            return damage;
        }

        public virtual int TakeDamageMod(int damage)
        {
            return damage;
        }

        public void SetParent(Card parent)
        {
            Parent = parent;
        }
    }
}