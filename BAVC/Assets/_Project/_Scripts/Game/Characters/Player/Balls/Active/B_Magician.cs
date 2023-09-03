using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct MagicianLevels
	{
		public float freezeTime;
		public float reappearTime;
	}
	
	public class B_Magician : Ball
	{
    	[Header("References")]
		[SerializeField] private Transform thisTransform;
		[SerializeField] private SpriteRenderer ballRenderer;
		[SerializeField] private GameObject ballSpriteGameObject;
		[SerializeField] private CircleCollider2D ballCollider;
		[SerializeField] private GameObject freezeEffectPrefab;
		[SerializeField] private ParticleSystem trailParticles;
		[SerializeField] private GameObject particleObject;

		[Header("Fields")]
		[SerializeField] private float popUpTime;
		[SerializeField] private float freezeEffectDestroyTime;
		
		[Space(10)]
		[SerializeField] private MagicianLevels[] magicianLevelsArray;

		private float _currentTimeToReappear;
		private float _currentFreezeTime;
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
			_currentTimeToReappear = magicianLevelsArray[_currentLevel - 1].reappearTime;
			ballRenderer.color = ballColor;
			_timeToReappearCounter = magicianLevelsArray[_currentLevel - 1].reappearTime;
			_isHidden = false;
			_defaultBallScale = thisTransform.localScale;
			_currentFreezeTime = magicianLevelsArray[_currentLevel - 1].freezeTime;
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
			_currentFreezeTime = magicianLevelsArray[_currentLevel - 1].freezeTime * _currentMultiplier;
			_currentTimeToReappear = magicianLevelsArray[_currentLevel - 1].reappearTime;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent(out IFreezable ifa))
				return;

			SoundManager.Instance.PlaySound(SoundType.Explosion);
			CameraFollow.Instance.ScreenShake();
			
			GameObject freezingEffect = Instantiate(freezeEffectPrefab, thisTransform.position, Quaternion.identity);
			freezingEffect.GetComponent<E_Freezing>().ConfigureEffect(_currentFreezeTime, ballColor, freezeEffectDestroyTime);

			Hide();
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentFreezeTime = magicianLevelsArray[_currentLevel - 1].freezeTime * _currentMultiplier;
		}
	}
}