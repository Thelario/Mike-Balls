using UnityEngine;

namespace Game
{
	public class B_Banker : Ball
	{
		[Header("References")]
		[SerializeField] private SpriteRenderer ballRenderer;
		
		private void Start()
		{
			ConfigureBall();
		}
        
		public override void ConfigureBall()
		{
			_currentLevel = 0;
			ballRenderer.color = ballColor;
			Upgrade();
		}
        
		public override void Upgrade()
		{
			_currentLevel++;
			CurrencyManager.Instance.Upgrade();
		}
	}
}