using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct FighterLevels
	{
		public float damage;
		public float reappearTime;
		public float knockoutStrenth;
		public float knockoutTime;
	}
	
	public class B_Fighter : Ball
	{
    	[Header("References")]
        [SerializeField] private Transform thisTransform;
        [SerializeField] private GameObject ballSpriteGameObject;
        [SerializeField] private CircleCollider2D ballCollider;
        [SerializeField] private ParticleSystem trailParticles;
        [SerializeField] private GameObject particleObject;
    
        [Header("Fields")]
        [SerializeField] private float popUpTime;

        [Space(10)]
        [SerializeField] private FighterLevels[] fighterLevels;

        private float _currentKnockoutStrength;
        private float _currentKnockoutTime;
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
            _currentTimeToReappear = fighterLevels[_currentLevel - 1].reappearTime;
            _timeToReappearCounter = fighterLevels[_currentLevel - 1].reappearTime;
            _currentKnockoutStrength = fighterLevels[_currentLevel - 1].knockoutStrenth;
            _currentDamage = fighterLevels[_currentLevel - 1].damage;
            _currentKnockoutTime = fighterLevels[_currentLevel - 1].knockoutTime;
            _isHidden = false;
            _hides = true;
            _defaultBallScale = thisTransform.localScale;
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
            _currentKnockoutStrength = fighterLevels[_currentLevel - 1].knockoutStrenth;
            _currentDamage = fighterLevels[_currentLevel - 1].damage * _currentMultiplier;
            _currentKnockoutTime = fighterLevels[_currentLevel - 1].knockoutTime;

            if (_currentLevel != 9)
            	return;
            
            _hides = false;
            Appear();
        }
        
        public override void UpgradeObject(float multiplier)
        {
            _currentMultiplier += multiplier;
            _currentDamage = fighterLevels[_currentLevel - 1].damage * _currentMultiplier;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IPushable ip))
            	return;
        
            ip.Push(_currentKnockoutStrength, _currentKnockoutTime);
            other.GetComponent<IDamageable>().TakeDamage(_currentDamage);
            
            SoundManager.Instance.PlaySound(SoundType.FighterHitSound);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectMedium, other.ClosestPoint(thisTransform.position), ballColor);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, ballColor);
            CameraFollow.Instance.ScreenShake();
            
            if (_hides)
            	Hide();
        }
	}
}