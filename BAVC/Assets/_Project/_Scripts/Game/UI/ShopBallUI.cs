using System.Collections;
using Game.Managers;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game
{
	public class ShopBallUI : MonoBehaviour
	{
		[SerializeField] private GameObject front;
		[SerializeField] private Image backImage;
		[SerializeField] private GameObject uiParticles;
		
		[Header("Shop Item Fields")]
		[SerializeField] private TMP_Text title;
		[SerializeField] private TMP_Text active;
		[SerializeField] private TMP_Text description;
		[SerializeField] private TMP_Text costText;
		[SerializeField] private Image itemImage;
		[SerializeField] private Image frontImage;
		[SerializeField] private Image costImage;
		
		private BallSO _ballSO;

		public void ConfigureShopItem(BallSO ballSO)
		{
			_ballSO = ballSO;
			
			title.text = ballSO.ballName;
			title.color = ballSO.ballColor;

			string text = ballSO.ballActive ? "Active" : "Passive";
			text += " - " + ballSO.ballClass;
			active.text = text;
			active.color = ballSO.ballActiveColor;

			description.text = ballSO.ballDescription;

			costText.text = "" + ballSO.ballPurchaseCost;
			
			itemImage.sprite = ballSO.ballSprite;
			itemImage.color = ballSO.ballActiveColor;
			
			costImage.color = ballSO.ballCostBackColor;

			frontImage.color = ballSO.ballFrontColor;
		}

		public void Purchase()
		{
			if (CurrencyManager.Instance.CanPurchase(_ballSO.ballPurchaseCost))
			{
				if (PartyManager.Instance.CheckBallExists(_ballSO.ballActive, _ballSO.ballType))
				{
					if (PartyManager.Instance.CanUpgradeBall(_ballSO.ballActive, _ballSO.ballType))
					{
						SoundManager.Instance.PlaySound(SoundType.Purchase);
						PartyManager.Instance.UpgradeBall(_ballSO.ballActive, _ballSO.ballType);
						CurrencyManager.Instance.SubstractMoney(_ballSO.ballPurchaseCost);
						ShopManagerUI.Instance.UpdateGoldText();
						PartyManagerUI.Instance.CreateBalls();
						StartCoroutine(Co_CreateParticles(_ballSO.ballColor));
						DeactivateShopItem();
					}
					else
					{
						ErrorWhenPurchasing();
					}
				}
				else
				{
					if (PartyManager.Instance.CanCreateBall(_ballSO.ballActive))
					{
						SoundManager.Instance.PlaySound(SoundType.Purchase);
						PartyManager.Instance.CreateBall(_ballSO.ballActive, _ballSO.ballPrefab);
						CurrencyManager.Instance.SubstractMoney(_ballSO.ballPurchaseCost);
						ShopManagerUI.Instance.UpdateGoldText();
						PartyManagerUI.Instance.CreateBalls();
						StartCoroutine(Co_CreateParticles(_ballSO.ballColor));
						DeactivateShopItem();
					}
					else
					{
						ErrorWhenPurchasing();
					}
				}
			}
			else
			{
				ErrorWhenPurchasing();
			}
		}

		private IEnumerator Co_CreateParticles(Color color)
		{
			GameObject particles = Instantiate(uiParticles, transform);
			particles.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
			foreach (Transform t in particles.transform)
				t.GetComponent<Image>().color = color;

			yield return new WaitForSecondsRealtime(0.3f);
			
			Destroy(particles);
		}

		private void ErrorWhenPurchasing()
		{
			SoundManager.Instance.PlaySound(SoundType.Error);
		}

		private void DeactivateShopItem()
		{
			front.SetActive(false);
			backImage.enabled = false;
			ShopManagerUI.Instance.EnableButtons(true);
		}
	}
}