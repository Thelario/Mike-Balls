using UnityEngine;

namespace Game
{
	public class EnemyHitWorldText : MonoBehaviour
	{
		[Header("References")] 
		[SerializeField] private RectTransform thisTransform;

		[Header("Move Upwards Animation")]
		[SerializeField] private float moveUpMinX;
		[SerializeField] private float moveUpMaxX;
		[SerializeField] private float moveUpY;
		[SerializeField] private float timeToMove;
		
		[Header("Popup Animation")]
		[SerializeField] private Vector3 maxScale;
		[SerializeField] private Vector3 endScale;
		[SerializeField] private float timeToMaxScale;
		[SerializeField] private float timeToDefaultScale;

		private void Start()
		{
			PopUp();
		}
		
		private void PopUp()
		{
			Vector2 positionToMove = thisTransform.position + new Vector3(Random.Range(moveUpMinX, moveUpMaxX), moveUpY);
			LeanTween.move(thisTransform, positionToMove, timeToMove);
			LeanTween.scale(thisTransform, maxScale, timeToMaxScale).setOnComplete(PopDown);
		}

		private void PopDown()
		{
			LeanTween.scale(thisTransform, endScale, timeToDefaultScale);
		}
	}
}