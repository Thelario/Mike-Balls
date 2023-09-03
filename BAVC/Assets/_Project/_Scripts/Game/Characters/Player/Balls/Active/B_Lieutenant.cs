using System.Collections;
using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct LieutenantLevels
	{
		public float fireRate;
		public float damage;
		public float range;
		public float speed;
	}
	
	public class B_Lieutenant : Ball
	{
    	[Header("ShootPoint Animation")]
		[SerializeField] private Vector3 shootPointScaleWhenAnimating;
		[SerializeField] private float shootPointAnimationTime;

		[Header("References")]
		[SerializeField] private Transform shootPointTransform;
		[SerializeField] private GameObject explosiveBulletPrefab;
		[SerializeField] private SpriteRenderer ballRenderer;
		[SerializeField] private SpriteRenderer shootPointRenderer;
		[SerializeField] private ParticleSystem trailParticles;
		[SerializeField] private float destroyTime;
		
		[Space(10)]
		[SerializeField] private LieutenantLevels[] lieutenantLevels;

		private float _currentFireRate;
		private float _currentDamage;
		private float _currentRange;
		private float _currentSpeed;
		private float _fireRateCounter;
		private Vector3 _defaultScale;

		private void Start()
		{
			ConfigureBall();
		}

		public override void ConfigureBall()
		{
			_currentLevel = 1;
			_currentMultiplier = PartyManager.Instance.GetMultiplier(ballClass);
			print(_currentMultiplier);
			_currentFireRate = lieutenantLevels[_currentLevel - 1].fireRate;
			_currentDamage = lieutenantLevels[_currentLevel - 1].damage;
			_currentRange = lieutenantLevels[_currentLevel - 1].range;
			_currentSpeed = lieutenantLevels[_currentLevel - 1].speed;
			_fireRateCounter = _currentFireRate;
			ballRenderer.color = ballColor;
			shootPointRenderer.color = ballColor;
			_defaultScale = shootPointTransform.localScale;
			trailParticles.startColor = ballColor;
		}

		private void Update()
		{
			_fireRateCounter -= Time.deltaTime;
			if (_fireRateCounter <= 0f)
				Shoot();
		}

		private void Shoot()
		{
			_fireRateCounter = _currentFireRate;
			
			StartCoroutine(Co_AnimateShootPoint());
			
			GameObject bullet = Instantiate(explosiveBulletPrefab, shootPointTransform.position, 
				Quaternion.Euler(shootPointTransform.rotation.eulerAngles.x, shootPointTransform.rotation.eulerAngles.y, shootPointTransform.rotation.eulerAngles.z + 90f));
			bullet.GetComponent<E_ExplosiveBullet>().ConfigureBullet(_currentDamage, _currentRange, _currentSpeed, ballColor, destroyTime);
			
			SoundManager.Instance.PlaySound(SoundType.Hit);
		}

		private IEnumerator Co_AnimateShootPoint()
		{
			shootPointTransform.localScale = shootPointScaleWhenAnimating;

			yield return new WaitForSeconds(shootPointAnimationTime);

			shootPointTransform.localScale = _defaultScale;
		}
		
		public override void Upgrade()
		{
			_currentLevel++;
			_currentFireRate = lieutenantLevels[_currentLevel - 1].fireRate;
			_currentDamage = lieutenantLevels[_currentLevel - 1].damage * _currentMultiplier;
			_currentRange = lieutenantLevels[_currentLevel - 1].range;
			_currentSpeed = lieutenantLevels[_currentLevel - 1].speed;
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentDamage = lieutenantLevels[_currentLevel - 1].damage * _currentMultiplier;
		}
	}
}