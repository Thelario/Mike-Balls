using Game.Managers;
using UnityEngine;

namespace Game
{
	public class E_Laser : MonoBehaviour
	{
		private float _damage;
		private Color _color;

		private void Update()
		{
			SoundManager.Instance.PlaySound(SoundType.Laser);
		}

		public void ConfigureLaser(float damage, Color color)
		{
			_damage = damage;
			_color = color;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent(out IDamageable id))
				return;
			
			id.TakeDamage(_damage);
			SoundManager.Instance.PlaySound(SoundType.Damage);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectSmall, other.ClosestPoint(transform.position), _color);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, _color);
		}
	}
}