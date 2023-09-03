using UnityEngine;

namespace Game
{
	public enum RotationState { Right, Left, Stopped }
	
	public class RandomRotationObject : MonoBehaviour
	{
		[SerializeField] private float minChangeStateTime;
		[SerializeField] private float maxChangeStateTime;
		[SerializeField] private float rotationSpeed;
		[SerializeField] private Transform thisTransform;

		private float _changeStateCounter;
		private RotationState _rotationState;

		private void Start()
		{
			_changeStateCounter = Random.Range(minChangeStateTime, maxChangeStateTime);
			_rotationState = (RotationState)Random.Range(0, 3);
		}

		private void Update()
		{
			CheckRotation();
			Rotate();
		}

		private void CheckRotation()
		{
			_changeStateCounter -= Time.deltaTime;
			if (_changeStateCounter <= 0f)
				ChangeState();
		}

		private void Rotate()
		{
			switch (_rotationState)
			{
				case RotationState.Left:
					thisTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
					break;
				case RotationState.Right:
					thisTransform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime, Space.Self);
					break;
				case RotationState.Stopped:
					return;
				default:
					thisTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
					break;
			}
		}

		private void ChangeState()
		{
			_rotationState = (RotationState)Random.Range(0, 3);
			_changeStateCounter = Random.Range(minChangeStateTime, maxChangeStateTime);
		}
	}
}