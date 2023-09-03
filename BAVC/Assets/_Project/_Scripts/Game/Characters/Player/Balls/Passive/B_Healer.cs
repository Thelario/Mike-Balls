using UnityEngine;

namespace Game
{
	public class B_Healer : Ball
	{
		[Header("References")]
		[SerializeField] private SpriteRenderer ballRenderer;
		
		private void Start()
		{
			XpManager.OnLevelUp += Heal;
			ConfigureBall();
		}

		private void OnDestroy()
		{
			XpManager.OnLevelUp -= Heal;
		}

		public override void ConfigureBall()
		{
			_currentLevel = 0;
			ballRenderer.color = ballColor;
			Upgrade();
		}

		private void Heal()
		{
			if (_currentLevel < 9)
				return;
			
			PlayerStats.Instance.Heal(1);
		}

		public override void Upgrade()
		{
			_currentLevel++;
			PlayerStats.Instance.Upgrade(1, 1);
		}
	}
}