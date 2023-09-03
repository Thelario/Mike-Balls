using UnityEngine;

namespace Game
{
	public enum BallType
	{
		Warrior, Archer, Bomberman, Magician, Pyromaniac, Guru, Healer, Banker, Siner, Executioner,
		Engineer, Swordsman, Lieutenant, Fighter, Boxer, Sniper, Terrorist, Wizard, Madman
	}

	public enum BallClass
	{
		Basic, Physical, Magical, Shooter, Explosive
	}
	
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ball", order = 1)]
	public class BallSO : ScriptableObject
	{
		public string ballName;
		[TextAreaAttribute] public string ballDescription;
		public int ballPurchaseCost;
		public bool ballActive;
		public BallType ballType;
		public BallClass ballClass;
		public Color ballColor;
		public Color ballFrontColor;
		public Color ballActiveColor;
		public Color ballCostBackColor;
		public GameObject ballPrefab;
		public Sprite ballSprite;
		public bool canDealDamage;
	}
}