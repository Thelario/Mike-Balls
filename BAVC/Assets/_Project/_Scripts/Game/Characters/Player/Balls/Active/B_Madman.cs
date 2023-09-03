using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct MadmanLevels
	{
		public float fireRate;
		public float damage;
	}
	
	public class B_Madman : Ball
	{
		[SerializeField] private float laserActiveTime;
		
		[Header("References")]
		[SerializeField] private GameObject laserObject;
		[SerializeField] private E_Laser laser;

		[Space(10)]
		[SerializeField] private MadmanLevels[] madmanLevels;

		private float _currentDamage;
		private float _currentFireRate;
		private float _fireRateCounter;
		private float _laserActiveTimeCounter;
		private bool _laserIsActive;

		private void Start()
		{
			ConfigureBall();
		}

		public override void ConfigureBall()
		{
			_currentLevel = 1;
			_currentMultiplier = PartyManager.Instance.GetMultiplier(ballClass);
			print(_currentMultiplier);
			_currentFireRate = madmanLevels[_currentLevel - 1].fireRate;
			_currentDamage = madmanLevels[_currentLevel - 1].damage;

			DisableLaser();
		}
		
		private void Update()
		{
			if (_laserIsActive)
			{
				_laserActiveTimeCounter -= Time.deltaTime;
				if (_laserActiveTimeCounter <= 0f)
					DisableLaser();
			}
			else
			{
				_fireRateCounter -= Time.deltaTime;
				if (_fireRateCounter <= 0f)
					EnableLaser();
			}
		}

		private void DisableLaser()
		{
			_fireRateCounter = _currentFireRate;
			_laserActiveTimeCounter = laserActiveTime;
			_laserIsActive = false;
			laserObject.SetActive(false);
		}

		private void EnableLaser()
		{
			_fireRateCounter = _currentFireRate;
			_laserActiveTimeCounter = laserActiveTime;
			_laserIsActive = true;
			laserObject.SetActive(true);
			laser.ConfigureLaser(_currentDamage, ballColor);
		}
		
		public override void Upgrade()
		{
			_currentLevel++;
			_currentFireRate = madmanLevels[_currentLevel - 1].fireRate;
			_currentDamage = madmanLevels[_currentLevel - 1].damage * _currentMultiplier;
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentDamage = madmanLevels[_currentLevel - 1].damage * _currentMultiplier;
		}
	}
}