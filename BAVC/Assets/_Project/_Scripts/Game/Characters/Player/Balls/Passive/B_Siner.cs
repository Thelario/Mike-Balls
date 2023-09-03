using Game.Managers;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct SinerLevels
    {
        public float fireRate;
        public float damage;
        public float range;
        public float speed;
        public int numberOfBullets;
    }
    
    public class B_Siner : Ball
    {
        [Header("References")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private SpriteRenderer ballRenderer;
		
        [Space(10)]
        [SerializeField] private SinerLevels[] sinerLevelsArray;
        
        private float _currentFireRate;
        private float _currentDamage;
        private float _currentRange;
        private float _currentSpeed;
        private float _fireRateCounter;
        private int _currentNumberOfBullets;
        
        private void Start()
        {
            ConfigureBall();
        }

        public override void ConfigureBall()
        {
            _currentLevel = 1;
            _currentMultiplier = PartyManager.Instance.GetMultiplier(ballClass);
            _currentFireRate = sinerLevelsArray[_currentLevel - 1].fireRate;
            _currentDamage = sinerLevelsArray[_currentLevel - 1].damage;
            _currentRange = sinerLevelsArray[_currentLevel - 1].range;
            _currentSpeed = sinerLevelsArray[_currentLevel - 1].speed;
            _currentNumberOfBullets = 4;
            _fireRateCounter = _currentFireRate;
            ballRenderer.color = ballColor;
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

            float rotationRate = 360f / _currentNumberOfBullets;
            for (int i = 0; i < _currentNumberOfBullets; i++)
            {
                ShootSingleBullet(PlayerMovement.Instance.PlayerPosition, Quaternion.Euler(0f, 0f, rotationRate * i));
            }
			
            SoundManager.Instance.PlaySound(SoundType.Damage);
        }

        private void ShootSingleBullet(Vector3 position, Quaternion rotation)
        {
            GameObject bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.GetComponent<E_Bullet>().ConfigureBullet(_currentDamage, _currentRange, _currentSpeed, ballColor);
        }
        
        public override void Upgrade()
        {
            _currentLevel++;
            _currentFireRate = sinerLevelsArray[_currentLevel - 1].fireRate;
            _currentDamage = sinerLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
            _currentRange = sinerLevelsArray[_currentLevel - 1].range;
            _currentSpeed = sinerLevelsArray[_currentLevel - 1].speed;
            _currentNumberOfBullets = sinerLevelsArray[_currentLevel - 1].numberOfBullets;
        }
        
        public override void UpgradeObject(float multiplier)
        {
            _currentMultiplier = multiplier;
            _currentDamage = sinerLevelsArray[_currentLevel - 1].damage * _currentMultiplier;
        }
    }
}