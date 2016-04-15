using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class Ship : MonoBehaviour
    {
        public GameObject Body;
        public GameObject Engine;
        public GameObject Exposion;
        public float Hp;
        public bool Invincible;
        public GameObject WrapIn;

        private void Start()
        {
            StartWrapIn();
        }

        private void StartWrapIn()
        {
            WrapIn.SetActive(true);
            iTween.ScaleFrom(Body, iTween.Hash("scale", Vector3.zero, "time", 1F, "delay", 3F,
                "oncomplete", "AfterWrapIn", "oncompletetarget", gameObject));
        }

        private void AfterWrapIn()
        {
            Engine.SetActive(true);
            foreach (var weapon in GetComponentsInChildren<Turret>())
            {
                weapon.enabled = true;
            }
            var path = GetComponent<iTweenMoveOnPath>();
            if (path != null)
                path.enabled = true;
        }

        public void TakeDamage(float damage)
        {
            if (Invincible) return;
            Hp -= damage;
            if (Hp > 0) return;
            if (Exposion == null) return;
            var exposion = Instantiate(Exposion);
            exposion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}