using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game
{
    public enum ParticleType
    {
        BulletDestroyedParticles, EnemyDeathParticles, EnemyHitParticles, EnemyHitEffectSmall,
        EnemyHitEffectMedium, EnemyHitEffectLarge, BossDeathParticles
    }

    [System.Serializable]
    public struct Particle
    {
        public ParticleType particleType;
        public GameObject particlePrefab;
        public float timeToDestroy;
    }
    
    public class ParticlesManager : Singleton<ParticlesManager>
    {
        [SerializeField] private Particle[] particles;
        
        private Dictionary<ParticleType, Particle> _particlesDictionary;

        private void Start()
        {
            _particlesDictionary = new Dictionary<ParticleType, Particle>();
            foreach (Particle particle in particles)
                _particlesDictionary.Add(particle.particleType, particle);
        }

        private Particle GetParticle(ParticleType particleType)
        {
            return _particlesDictionary[particleType];
        }

        public void CreateParticle(ParticleType particleType, Vector3 position, Color color)
        {
            Particle particle = GetParticle(particleType);
            
            if (particleType is ParticleType.EnemyHitEffectSmall or ParticleType.EnemyHitEffectMedium or ParticleType.EnemyHitEffectLarge)
            {
                GameObject hitEffect = Instantiate(particle.particlePrefab, position, Quaternion.identity);
                hitEffect.GetComponent<SpriteRenderer>().color = color;
                Destroy(hitEffect, particle.timeToDestroy);
            }
            else
            {
                GameObject hitParticles = Instantiate(particle.particlePrefab, position, Quaternion.identity);
                hitParticles.GetComponent<ParticleSystem>().startColor = color;
                Destroy(hitParticles, particle.timeToDestroy);
            }
        }
    }
}