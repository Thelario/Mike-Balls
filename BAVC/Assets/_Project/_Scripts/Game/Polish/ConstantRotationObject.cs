using UnityEngine;

namespace Game
{
	public class ConstantRotationObject : MonoBehaviour
	{
		[SerializeField] private float rotationSpeed;
		[SerializeField] private bool ignoreTimeScale;
		[SerializeField] private RectTransform rectTransform;
		
		private void Update()
		{
			if (ignoreTimeScale)
				rectTransform.Rotate(0f, 0f, rotationSpeed, Space.Self);
			else
				rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
		}
	}
}