using Game.Managers;
using UnityEngine;

namespace Game
{
    public class ChaserBoss : Enemy
    {
        protected override void Die()
        {
            SoundManager.Instance.PlaySound(SoundType.BossDeath);
            ParticlesManager.Instance.CreateParticle(ParticleType.BossDeathParticles, thisTransform.position, defaultEnemyColor);
            ItemsManager.Instance.OpenItemsPanel(1f);
            Destroy(gameObject);
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            PlayerStats.Instance.TakeDamage(_currentDamage);
        }
    }
}