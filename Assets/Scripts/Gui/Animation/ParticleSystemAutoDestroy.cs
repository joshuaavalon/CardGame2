using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gui.Animation
{
    public class ParticleSystemAutoDestroy : MonoBehaviour
    {
        private ParticleSystem[] _particleSystems;

        public void Start()
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        public void Update()
        {
            if (!_particleSystems.Any(ps => ps.IsAlive()))
                Destroy(gameObject);
        }
    }
}
