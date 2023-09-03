using UnityEngine;

namespace Game
{
	public class Ball : MonoBehaviour, IUpgradable
	{
		[SerializeField] protected BallClass ballClass;
		[SerializeField] protected BallType ballType;
		[SerializeField] protected Color ballColor;

		protected int _currentLevel = 1;
		protected float _currentMultiplier = 1f;

		public virtual void ConfigureBall()
		{
			_currentLevel = 1;
			_currentMultiplier = 1f;
		}
		
		public virtual void Upgrade()
		{
			_currentLevel++;
		}

		public bool CheckBallType(BallType type)
		{
			return ballType == type;
		}
		
		public bool CheckBallClass(BallClass bClass)
		{
			return ballClass == bClass;
		}

		public int GetLevel()
		{
			return _currentLevel;
		}

		public Color GetColor()
		{
			return ballColor;
		}

		public virtual void UpgradeObject(float multiplier)
		{
			_currentMultiplier = multiplier;
		}
	}
}