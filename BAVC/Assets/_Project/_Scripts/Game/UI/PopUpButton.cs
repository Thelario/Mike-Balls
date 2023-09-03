using Game.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
	public class PopUpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[Header("PopUp Animation Fields")] 
		[SerializeField] private RectTransform thisRectransform;
		[SerializeField] private Vector3 defaultScale;
		[SerializeField] private Vector3 maxScale;
		[SerializeField] private float timeFromDefaultToMaxScale;

		public void OnPointerEnter(PointerEventData eventData)
		{
			LeanTween.scale(thisRectransform, maxScale, timeFromDefaultToMaxScale).setIgnoreTimeScale(true);
			SoundManager.Instance.PlaySound(SoundType.Blop);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			LeanTween.scale(thisRectransform, defaultScale, timeFromDefaultToMaxScale).setIgnoreTimeScale(true);
		}
	}
}