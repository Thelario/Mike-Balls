using UnityEngine;

namespace Game
{
	public class ShopUI : MonoBehaviour
	{
		private void OnDisable()
		{
			ShopManagerUI.Instance.DisableShop();
		}

		private void OnEnable()
		{
			ShopManagerUI.Instance.EnableShop();
		}
	}
}