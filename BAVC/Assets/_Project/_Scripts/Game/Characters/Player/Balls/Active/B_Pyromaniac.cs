using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct PyromaniacLevels
	{
		public float damagePerHit;
		public float reappearTime;
		public float timeBetweenHits;
	}
	
	public class B_Pyromaniac : Ball
	{
    	[Header("References")]
		[SerializeField] private Transform thisTransform;
		[SerializeField] private SpriteRenderer ballRenderer;
		[SerializeField] private GameObject ballSpriteGameObject;
		[SerializeField] private CircleCollider2D ballCollider;
		[SerializeField] private GameObject fireEffectPrefab;
		[SerializeField] private ParticleSystem trailParticles;
		[SerializeField] private GameObject particleObject;

		[Header("Fields")]
		[SerializeField] private float popUpTime;
		[SerializeField] private float fireEffectDestroyTime;

		[Space(10)]
		[SerializeField] private PyromaniacLevels[] pyromaniacLevelsArray;
		
		private float _currentTimeToReappear;
		private float _currentDamage;
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
			_currentTimeToReappear = pyromaniacLevelsArray[_currentLevel - 1].reappearTime;
			ballRenderer.color = ballColor;
			_timeToReappearCounter = pyromaniacLevelsArray[_currentLevel - 1].reappearTime;
			_isHidden = false;
			_defaultBallScale = thisTransform.localScale;
			_currentDamage = pyromaniacLevelsArray[_currentLevel - 1].damagePerHit;
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
			_currentDamage = pyromaniacLevelsArray[_currentLevel - 1].damagePerHit * _currentMultiplier;
			_currentTimeToReappear = pyromaniacLevelsArray[_currentLevel - 1].reappearTime;
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentDamage = pyromaniacLevelsArray[_currentLevel - 1].damagePerHit * _currentMultiplier;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent(out IDamageable ida))
				return;

			SoundManager.Instance.PlaySound(SoundType.FireBall);
			CameraFollow.Instance.ScreenShake();
			
			GameObject explosionEffect = Instantiate(fireEffectPrefab, thisTransform.position, Quaternion.identity);
			explosionEffect.GetComponent<E_Fire>().ConfigureEffect(_currentDamage, pyromaniacLevelsArray[_currentLevel - 1].timeBetweenHits, ballColor, fireEffectDestroyTime);

			Hide();
		}
	}
}