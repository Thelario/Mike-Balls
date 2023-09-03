using System.Collections;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class StartGameButton : MonoBehaviour
	{
		private Button _button;

		private void Start()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(StartGame);
		}

		private void StartGame()
		{
			StartCoroutine(nameof(Co_StartGame));
		}

		private IEnumerator Co_StartGame()
		{
			SoundManager.Instance.PlaySound(SoundType.ButtonClicked);
			TransitionsManager.Instance.Transition();

			yield return new WaitForSecondsRealtime(.5f);
			
			GameManager.Instance.StartGame();
		}
	}
}