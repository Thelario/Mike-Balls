using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct WarriorLevels
	{
		public float damage;
		public float reappearTime;
	}
	
	public class B_Warrior : Ball
	{
		[Header("References")]
		[SerializeField] private Transform thisTransform;
		[SerializeField] private SpriteRenderer ballRenderer;
		[SerializeField] private GameObject ballSpriteGameObject;
		[SerializeField] private CircleCollider2D ballCollider;
		[SerializeField] private ParticleSystem trailParticles;
		[SerializeField] private GameObject particleObject;
    
		[Header("Fields")]
		[SerializeField] private float popUpTime;

		[Space(10)]
		[SerializeField] private WarriorLevels[] warriorLevelsArray;
		
		private float _currentTimeToReappear;
		private float _currentDamage;
		private float _timeToReappearCounter;
		private bool _isHidden;
		private bool _hides;
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
			_currentTimeToReappear = warriorLevelsArray[_currentLevel - 1].reappearTime;
			ballRenderer.color = ballColor;
			_timeToReappearCounter = warriorLevelsArray[_currentLevel - 1].reappearTime;
			_isHidden = false;
			_hides = true;
			_defaultBallScale = thisTransform.localScale;
			_currentDamage = warriorLevelsArray[_currentLevel - 1].damage;
			trailParticles.startColor = ballColor;
		}

		private void Update()
		{
			if (!_isHidden || !_hides)
				return;

			_timeToReappearCounter -= Time.deltaTime;
			if (_timeToReappearCounter <= 0f)
				Appear();
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

		private void Hide()
		{
			_timeToReappearCounter = _currentTimeToReappear;
			_isHidden = true;
			ballSpriteGameObject.SetActive(false);
			ballCollider.enabled = false;
			particleObject.SetActive(false);
		}

		private void PopUp()
		{
			LeanTween.scale(gameObject, _defaultBallScale, popUpTime);
		}
		
		public override void Upgrade()
		{
			_currentLevel++;
			_currentDamage = warriorLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
			_currentTimeToReappear = warriorLevelsArray[_currentLevel - 1].reappearTime;

			if (_currentLevel != 9)
				return;
			
			_hides = false;
			Appear();
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentDamage = warriorLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent(out IDamageable ida))
				return;
        
			ida.TakeDamage(_currentDamage);
			
			SoundManager.Instance.PlaySound(SoundType.WarriorHitSound);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectMedium, other.ClosestPoint(thisTransform.position), ballColor);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, ballColor);
			CameraFollow.Instance.ScreenShake();
			
			if (_hides)
				Hide();
		}
	}
}