using Game.Managers;
using UnityEngine;

namespace Game
{
	public class ShooterBoss : Enemy
	{
		[Header("Stats")]
		[SerializeField] private float bulletSpeed;
		[SerializeField] private float bulletRange;
		[SerializeField] private float fireRate;
		[SerializeField] private Color bulletColor;
		
		[Header("References")]
		[SerializeField] private GameObject bossBullet;
		[SerializeField] private Transform shootPointsParent;

		private float _fireRateCounter;

		protected override void Update()
		{
			base.Update();

			_fireRateCounter -= Time.deltaTime;
			if (_fireRateCounter <= 0f)
				Shoot();
		}

		private void Shoot()
		{
			_fireRateCounter = fireRate;
			
			foreach (Transform shootPoint in shootPointsParent)
				ShootIndividualBullet(shootPoint);
		}

		private void ShootIndividualBullet(Transform shootPoint)
		{
			GameObject bullet = Instantiate(bossBullet, shootPoint.position, 
				Quaternion.Euler(thisTransform.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z + 90f));
			bullet.GetComponent<EnemyBullet>().ConfigureBullet(_currentDamage, bulletRange, bulletSpeed, bulletColor);
			
			SoundManager.Instance.PlaySound(SoundType.Hit);
		}

		protected override void Die()
		{
			SoundManager.Instance.PlaySound(SoundType.BossDeath);
			ParticlesManager.Instance.CreateParticle(ParticleType.BossDeathParticles, thisTransform.position, defaultEnemyColor);
			ItemsManager.Instance.OpenItemsPanel(.75f);
			Destroy(gameObject);
		}

		protected override void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.gameObject.CompareTag("Player"))
				return;
            
			PlayerStats.Instance.TakeDamage(damage);
		}
	}
}