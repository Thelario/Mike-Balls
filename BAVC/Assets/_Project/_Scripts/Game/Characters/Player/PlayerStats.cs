using System.Collections;
using Game.Managers;
using Game.UI;
using UnityEngine;

namespace Game
{
	public class PlayerStats : Singleton<PlayerStats>
	{
		public delegate void OnPlayerHit();
		public static event OnPlayerHit PlayerHit;
		
		[Header("Stats")]
		[SerializeField] private int maxHealth;
		[SerializeField] private float moveSpeed;
		[SerializeField] private float ballsRotationSpeed;

		[Header("Fields")]
		[SerializeField] private float invencibilityTime;

		[Header("Prefabs")]
		[SerializeField] private GameObject fullHeartPrefab;
		[SerializeField] private GameObject emptyHeartPrefab;

		[Header("References")]
		[SerializeField] private RectTransform heartsParent;
		[SerializeField] private Transform activeBallsParent;
		
		[Header("Player Hit Animation")]
		[SerializeField] private GameObject hitPanel;
		[SerializeField] private float hitTime;
		
		private int _currentHealth;
		private int _currentMaxHealth;
		private float _currentMoveSpeed;
		private float _currentBallsRotationSpeed;
		private float _invencibilityTimeCounter;
		private bool _playerIsInvencible;

		public float MoveSpeed => _currentMoveSpeed;
		public float BallRotationSpeed => _currentBallsRotationSpeed;

		public void ConfigurePlayerStats()
		{
			_currentMaxHealth = maxHealth;
			_currentHealth = _currentMaxHealth;
			_currentMoveSpeed = moveSpeed;
			_currentBallsRotationSpeed = ballsRotationSpeed;
			_invencibilityTimeCounter = invencibilityTime;
			hitPanel.SetActive(false);
			
			UpdateHealthUI();
		}

		private void Update()
		{
			if (!_playerIsInvencible)
				return;
			
			_invencibilityTimeCounter -= Time.deltaTime;
			if (_invencibilityTimeCounter > 0f)
				return;

			_playerIsInvencible = false;
		}

		private void UpdateHealthUI()
		{
			DestroyHearts();

			for (int i = 0; i < _currentHealth; i++)
				Instantiate(fullHeartPrefab, heartsParent);
			
			for (int j = 0; j < _currentMaxHealth - _currentHealth; j++)
				Instantiate(emptyHeartPrefab, heartsParent);
		}

		private void DestroyHearts()
		{
			if (heartsParent.childCount <= 0)
				return;
			
			foreach (RectTransform heart in heartsParent)
				Destroy(heart.gameObject);
		}
		
		public void Heal(int healAmount)
		{
			_currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _currentMaxHealth);
			
			UpdateHealthUI();
		}

		public void TakeDamage(int damage)
		{
			if (_playerIsInvencible)
				return;
			
			SoundManager.Instance.PlaySound(SoundType.PlayerHit);
			_currentHealth -= damage;
			_invencibilityTimeCounter = invencibilityTime;
			_playerIsInvencible = true;
			PlayerHit?.Invoke();
			
			UpdateHealthUI();

			StartCoroutine(Co_PlayerHitAnimation());
		}

		private IEnumerator Co_PlayerHitAnimation()
		{
			hitPanel.SetActive(true);
			TimeManager.Instance.Pause();

			yield return new WaitForSecondsRealtime(hitTime);
			
			hitPanel.SetActive(false);

			if (_currentHealth <= 0)
				Die();
			else
				TimeManager.Instance.Resume();
		}
		
		private void Die()
		{
			SoundManager.Instance.PlaySound(SoundType.Lose);
			CameraFollow.Instance.StopScreenShake();
			CanvasManager.Instance.SwitchCanvas(CanvasType.GameLostMenu);
			TimeManager.Instance.Pause();
		}

		public void Upgrade(float moveSpeedAmount, float rotationSpeedAmount, float crossDistance, Vector2 sidesDistance)
		{
			_currentMoveSpeed += moveSpeedAmount;
			_currentBallsRotationSpeed += rotationSpeedAmount;

			foreach (Transform ballParent in activeBallsParent)
				ballParent.GetComponent<BallParent>().Upgrade(crossDistance, sidesDistance);
		}
		
		public void Upgrade(int healAmount, int maxHealthAmount)
		{
			_currentMaxHealth += maxHealthAmount;
			Heal(healAmount);
		}
	}
}