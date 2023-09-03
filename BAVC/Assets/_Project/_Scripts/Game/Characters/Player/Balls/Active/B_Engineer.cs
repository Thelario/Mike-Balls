using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct EngineerLevels
	{
		public float damage;
		public float range;
		public float speed;
		public float reappearTime;
	}
	
	public class B_Engineer : Ball
	{
    	[Header("References")]
		[SerializeField] private Transform shootPointTransform;
		[SerializeField] private Transform thisTransform;
		[SerializeField] private GameObject bulletPrefab;
		[SerializeField] private SpriteRenderer ballRenderer;
		[SerializeField] private ParticleSystem trailParticles;
		[SerializeField] private GameObject ballSpriteGameObject;
		[SerializeField] private CircleCollider2D ballCollider;
		[SerializeField] private GameObject particleObject;
		
		[Header("Fields")]
		[SerializeField] private float popUpTime;
		
		[Space(10)]
		[SerializeField] private EngineerLevels[] engineerLevelsArray;

		private float _currentDamage;
		private float _currentRange;
		private float _currentSpeed;
		private float _currentTimeToReappear;
		private float _timeToReappearCounter;
		private bool _isHidden;
		private Vector3 _defaultBallScale;

		private void Start()
		{
			ConfigureBall();
		}

		public override void ConfigureBall()
		{
			_currentLevel = 1;
			_currentMultiplier = PartyManager.Instance.GetMultiplier(ballClass);
			print(_currentMultiplier);
			_currentTimeToReappear = engineerLevelsArray[_currentLevel - 1].reappearTime;
			_currentDamage = engineerLevelsArray[_currentLevel - 1].damage;
			_currentRange = engineerLevelsArray[_currentLevel - 1].range;
			_currentSpeed = engineerLevelsArray[_currentLevel - 1].speed;
			ballRenderer.color = ballColor;
			_defaultBallScale = thisTransform.localScale;
			trailParticles.startColor = ballColor;
		}
		
		private void Update()
		{
			if (!_isHidden)
				return;

			_timeToReappearCounter -= Time.deltaTime;
			if (_timeToReappearCounter <= 0f)
				Appear();
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent(out IDamageable ida))
				return;

			SoundManager.Instance.PlaySound(SoundType.Explosion);
			CameraFollow.Instance.ScreenShake();

			Shoot();
			
			Hide();
		}

		private void Shoot()
		{
			GameObject bullet = Instantiate(bulletPrefab, shootPointTransform.position, 
				Quaternion.Euler(shootPointTransform.rotation.eulerAngles.x, shootPointTransform.rotation.eulerAngles.y, shootPointTransform.rotation.eulerAngles.z + 90f));
			bullet.GetComponent<E_Whirlwind>().ConfigureWhirlwind(_currentDamage, _currentRange, _currentSpeed, ballColor);
		}
		
		private void Hide()
		{
			_timeToReappearCounter = _currentTimeToReappear;
			_isHidden = true;
			ballSpriteGameObject.SetActive(false);
			ballCollider.enabled = false;
			particleObject.SetActive(false);
		}
		
		private void Appear()
		{
			thisTransform.localScale = Vector3.zero;
			_timeToReappearCounter = _currentTimeToReappear;
			_isHidden = false;
			ballSpriteGameObject.SetActive(true);
			ballCollider.enabled = true;
			particleObject.SetActive(true);
			PopUp();
		}
		
		private void PopUp()
		{
			LeanTween.scale(gameObject, _defaultBallScale, popUpTime);
		}

		public override void Upgrade()
		{
			_currentLevel++;
			_currentTimeToReappear = engineerLevelsArray[_currentLevel - 1].reappearTime;
			_currentDamage = engineerLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
			_currentRange = engineerLevelsArray[_currentLevel - 1].range;
			_currentSpeed = engineerLevelsArray[_currentLevel - 1].speed;
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentDamage = engineerLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
		}
	}
}