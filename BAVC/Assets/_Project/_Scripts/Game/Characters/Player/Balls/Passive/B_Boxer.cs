using UnityEngine;

namespace Game
{
	public class B_Boxer : Ball
	{
		[SerializeField] private float multiplier;

		private void Start()
		{
			ConfigureBall();
		}
        
		public override void ConfigureBall()
		{
			_currentLevel = 0;
			Upgrade();
		}
        
		public override void Upgrade()
		{
			_currentLevel++;
			PartyManager.Instance.UpgradeBalls(multiplier, ballClass);
		}
	}
}