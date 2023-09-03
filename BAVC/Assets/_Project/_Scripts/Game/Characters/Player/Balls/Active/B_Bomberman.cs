using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct BombermanLevels
	{
		public float damage;
		public float reappearTime;
	}
	
	public class B_Bomberman : Ball
	{
		[Header("References")]
		[SerializeField] private Transform thisTransform;
		[SerializeField] private SpriteRenderer ballRenderer;
		[SerializeField] private GameObject ballSpriteGameObject;
		[SerializeField] private CircleCollider2D ballCollider;
		[SerializeField] private GameObject explosionEffectPrefab;
		[SerializeField] private ParticleSystem trailParticles;
		[SerializeField] private GameObject particleObject;

		[Header("Fields")]
		[SerializeField] private float popUpTime;
		[SerializeField] private float explosionDestroyTime;
		
		[Space(10)]
		[SerializeField] private BombermanLevels[] bombermanLevelsArray;

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
			_currentTimeToReappear = bombermanLevelsArray[_currentLevel - 1].reappearTime;
			ballRenderer.color = ballColor;
			_timeToReappearCounter = bombermanLevelsArray[_currentLevel - 1].reappearTime;
			_isHidden = false;
			_defaultBallScale = thisTransform.localScale;
			_currentDamage = bombermanLevelsArray[_currentLevel - 1].damage;
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
			_currentDamage = bombermanLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
			_currentTimeToReappear = bombermanLevelsArray[_currentLevel - 1].reappearTime;
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentDamage = bombermanLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent(out IDamageable ida))
				return;

			SoundManager.Instance.PlaySound(SoundType.Explosion);
			CameraFollow.Instance.ScreenShake();
			
			GameObject explosionEffect = Instantiate(explosionEffectPrefab, thisTransform.position, Quaternion.identity);
			explosionEffect.GetComponent<E_Explosion>().ConfigureEffect(_currentDamage, ballColor, explosionDestroyTime);
			
			Hide();
		}
	}
}