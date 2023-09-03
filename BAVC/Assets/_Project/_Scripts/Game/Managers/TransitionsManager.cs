using System.Collections;
using Game.Managers;
using UnityEngine;

namespace Game
{
	public class TransitionsManager : Singleton<TransitionsManager>
	{
		[SerializeField] private float transitionTime;
		[SerializeField] private Vector3 maxTransitionScale;
		[SerializeField] private GameObject[] figures;

		public void Transition()
		{
			StartCoroutine(Co_Scale());
		}

		private IEnumerator Co_Scale()
		{
			int figure = Random.Range(0, figures.Length);
			
			LeanTween.scale(figures[figure], maxTransitionScale, transitionTime).setIgnoreTimeScale(true);

			yield return new WaitForSecondsRealtime(transitionTime * 2f);
			
			LeanTween.scale(figures[figure], Vector3.zero, transitionTime).setIgnoreTimeScale(true);
		}
	}
}