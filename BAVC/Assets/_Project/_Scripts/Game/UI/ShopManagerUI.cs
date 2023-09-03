using System.Collections.Generic;
using Game.Managers;
using TMPro;
using UnityEngine;

namespace Game
{
	public class ShopManagerUI : Singleton<ShopManagerUI>
	{
		[Header("Shop Buttons")]
		[SerializeField] private GameObject rerollButton;
		[SerializeField] private GameObject fightButton;
		
		[Header("Shop References")]
		[SerializeField] private GameObject shopItemPrefab;
		[SerializeField] private RectTransform shopItemsParent;
		[SerializeField] private TMP_Text goldText;
		[SerializeField] private TMP_Text rerollText;
		[SerializeField] private List<BallSO> balls;

		private int _rerollCost = 2;
		private bool _isFirstTimeShoppingInWalmart;

		public void ConfigureShopManagerUI()
		{
			_rerollCost = 2;
			_isFirstTimeShoppingInWalmart = true;
			EnableButtons(false);
		}
		
		public void EnableShop()
		{
			goldText.text = "" + CurrencyManager.Instance.Money;
			rerollText.text = "" + _rerollCost;
			EnableButtons(!_isFirstTimeShoppingInWalmart);
			CreateShopItems();
		}

		public void DisableShop()
		{
			DestroyShopItems();

			SpawnerManager.Instance.PushAllEnemiesBackwards();
			
			_rerollCost = Mathf.Clamp(_rerollCost - 1, 2, 6);
		}

		public void UpdateGoldText()
		{
			goldText.text = "" + CurrencyManager.Instance.Money;
		}

		public void ReRoll()
		{
			if (CurrencyManager.Instance.CanPurchase(_rerollCost))
			{
				SoundManager.Instance.PlaySound(SoundType.Reroll);
				CurrencyManager.Instance.SubstractMoney(_rerollCost);
				_rerollCost = Mathf.Clamp(_rerollCost + 1, 2, 6);
				rerollText.text = "" + _rerollCost;
				goldText.text = "" + CurrencyManager.Instance.Money;
				CreateShopItems();
			}
			else
				SoundManager.Instance.PlaySound(SoundType.Error);
		}
		
		private void CreateShopItems()
		{
			DestroyShopItems();

			float extraTime = 0.15f;
			
			if (_isFirstTimeShoppingInWalmart)
			{
				for (int i = 0; i < 3; i++)
				{
					BallSO ballSO;
					do {
						ballSO = balls[Random.Range(0, balls.Count)];
					}
					while (!ballSO.canDealDamage || ballSO.ballPurchaseCost > 4);
					
					GameObject shopItem = Instantiate(shopItemPrefab, shopItemsParent);
					shopItem.transform.localScale = Vector3.zero;
					LeanTween.scale(shopItem, Vector3.one, extraTime).setIgnoreTimeScale(true).setDelay(.3f);
					shopItem.GetComponent<ShopBallUI>().ConfigureShopItem(ballSO);
					extraTime += .2f;
				}

				_isFirstTimeShoppingInWalmart = false;
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					BallSO ballSO = balls[Random.Range(0, balls.Count)];
					GameObject shopItem = Instantiate(shopItemPrefab, shopItemsParent);
					shopItem.transform.localScale = Vector3.zero;
					LeanTween.scale(shopItem, Vector3.one, extraTime).setIgnoreTimeScale(true);
					shopItem.GetComponent<ShopBallUI>().ConfigureShopItem(ballSO);
					extraTime += .2f;
				}
			}
		}

		public void EnableButtons(bool enable)
		{
			rerollButton.SetActive(enable);
			fightButton.SetActive(enable);
		}

		private void DestroyShopItems()
		{
			if (shopItemsParent == null)
				return;
			
			if (shopItemsParent.childCount == 0)
				return;
			
			foreach (RectTransform rt in shopItemsParent)
				Destroy(rt.gameObject);
		}
	}
}