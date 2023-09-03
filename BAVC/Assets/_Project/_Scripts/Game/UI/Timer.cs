using Game.Managers;
using TMPro;
using UnityEngine;

namespace Game
{
	public class Timer : Singleton<Timer>
	{
		[SerializeField] private TMP_Text timerText;

		private float _timeSinceGameStarted;
		private int _minutes;
		private int _seconds;
		private string _auxText;

		private void Update()
		{
			CalculateMinutesAndSeconds();

			SetTimerText();
		}

		private void CalculateMinutesAndSeconds()
		{
			_timeSinceGameStarted += Time.deltaTime;

			_minutes = Mathf.FloorToInt(_timeSinceGameStarted / 60);
			_seconds = Mathf.FloorToInt(_timeSinceGameStarted % 60);
		}

		private void SetTimerText()
		{
			_auxText = "";

			if (_minutes == 0)
				_auxText += "00:";
			else if (_minutes < 10)
				_auxText += "0" + _minutes + ":";
			else
				_auxText += _minutes + ":";

			if (_seconds < 10)
				_auxText += "0" + _seconds;
			else
				_auxText += "" + _seconds;
			
			timerText.text = _auxText;
		}
		
		public void ResetTimer()
		{
			_timeSinceGameStarted = 0;
			_minutes = 0;
		}
	}
}