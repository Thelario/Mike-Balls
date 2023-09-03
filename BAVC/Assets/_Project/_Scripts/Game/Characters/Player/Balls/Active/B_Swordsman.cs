using System.Collections;
using Game.Managers;
using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct SwordsmanLevels
	{
		public float damage;
		public float range;
		public float timeBetweenSwordWipes;
	}
	
	public class B_Swordsman : Ball
	{
		[Header("References")]
		[SerializeField] private GameObject swordPrefab;
		[SerializeField] private float swipeTime;

		[Space(10)]
		[SerializeField] private SwordsmanLevels[] swordsmanLevels;
		
		private float _currentDamage;
		private float _currentRange;
		private float _currentTimeBetweenSwordWipes;
		private float _currentTimeBetweenSwordWipesCounter;

		private void Start()
		{
			ConfigureBall();
		}

		public override void ConfigureBall()
		{
			_currentLevel = 1;
			_currentMultiplier = PartyManager.Instance.GetMultiplier(ballClass);
			print(_currentMultiplier);
			_currentDamage = swordsmanLevels[_currentLevel - 1].damage;
			_currentRange = swordsmanLevels[_currentLevel - 1].range;
			_currentTimeBetweenSwordWipes = swordsmanLevels[_currentLevel - 1].timeBetweenSwordWipes;
		}
		
		private void Update()
		{
			_currentTimeBetweenSwordWipesCounter -= Time.deltaTime;
			if (_currentTimeBetweenSwordWipesCounter <= 0f)
				CreateSwordWipe();
		}

		private void CreateSwordWipe()
		{
			Instantiate(swordPrefab, transform).GetComponent<E_Sword>()
				.ConfigureEffect(ballColor, _currentDamage, _currentRange, swipeTime, _currentTimeBetweenSwordWipes <= 0.5f);
			
			_currentTimeBetweenSwordWipesCounter = _currentTimeBetweenSwordWipes;

			StartCoroutine(nameof(Co_PlaySoundAfterTime), .5f);
		}

		private IEnumerator Co_PlaySoundAfterTime(float time)
		{
			yield return new WaitForSecondsRealtime(time);
			
			SoundManager.Instance.PlaySound(SoundType.SwordCreated);
		}

		public override void Upgrade()
		{
			_currentLevel++;
			_currentTimeBetweenSwordWipes = swordsmanLevels[_currentLevel - 1].timeBetweenSwordWipes;
			_currentDamage = swordsmanLevels[_currentLevel - 1].damage * _currentMultiplier;
			_currentRange = swordsmanLevels[_currentLevel - 1].range;
		}
		
		public override void UpgradeObject(float multiplier)
		{
			_currentMultiplier += multiplier;
			_currentDamage = swordsmanLevels[_currentLevel - 1].damage * _currentMultiplier;
		}
	}
}