using System.Collections.Generic;
using Game.Managers;
using TMPro;
using UnityEngine;

namespace Game
{
	public class PartyManagerUI : Singleton<PartyManagerUI>
	{
		[Header("References")]
		[SerializeField] private GameObject slotSmallPrefab;
		[SerializeField] private GameObject slotMediumPrefab;
		[SerializeField] private GameObject slotBigPrefab;
		[SerializeField] private GameObject ballsSlotPrefab;
		[SerializeField] private RectTransform activeBallsPanel;
		[SerializeField] private RectTransform passiveBallsPanel;

		[Header("Party Max Limit Texts")]
		[SerializeField] private TMP_Text activeText;
		[SerializeField] private TMP_Text passiveText;
		
		private void OnEnable()
		{
			CreateBalls();
		}

		private void OnDisable()
		{
			DestroyBalls();
		}

		public void CreateBalls()
		{
			DestroyBalls();

			CreateBallsWithinCorrectPanel(PartyManager.Instance.CurrentActiveParty, activeBallsPanel);
			CreateBallsWithinCorrectPanel(PartyManager.Instance.CurrentPassiveParty, passiveBallsPanel);

			activeText.text = PartyManager.Instance.CurrentActivePartyCount + "/" + PartyManager.Instance.MaxPartyLimit;
			passiveText.text = PartyManager.Instance.CurrentPassivePartyCount + "/" + PartyManager.Instance.MaxPartyLimit;
		}

		private void CreateBallsWithinCorrectPanel(List<GameObject> currentParty, RectTransform ballsPanel)
		{
			if (currentParty.Count == 0)
				return;
			
			foreach (GameObject ball in currentParty)
			{
				Transform slot = Instantiate(ballsSlotPrefab, ballsPanel).transform;
				Ball ballRef = ball.GetComponent<Ball>();
				int level = ballRef.GetLevel();
				if (level == 9)
				{
					Instantiate(slotBigPrefab, slot).GetComponent<SingleSlot>().ConfigureSlot(ballRef.GetColor());
				}
				else
				{
					int divisor = level / 3;
					int resto = level - divisor * 3;
					
					for (int i = 0; i < divisor; i++)
						Instantiate(slotMediumPrefab, slot).GetComponent<SingleSlot>().ConfigureSlot(ballRef.GetColor());

					for (int j = 0; j < resto; j++)
						Instantiate(slotSmallPrefab, slot).GetComponent<SingleSlot>().ConfigureSlot(ballRef.GetColor());
				}
			}
		}

		private void DestroyBalls()
		{
			if (activeBallsPanel.childCount > 0)
			{
				foreach (RectTransform rta in activeBallsPanel)
					Destroy(rta.gameObject);
			}

			if (passiveBallsPanel.childCount > 0)
			{
				foreach (RectTransform rtp in passiveBallsPanel)
					Destroy(rtp.gameObject);
			}
		}
	}
}