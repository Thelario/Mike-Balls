using UnityEngine;

namespace Game
{
	public enum BallParentType { Upper, RightUpper, Right, RightLower, Lower, LeftLower, Left, LeftUpper}
	
	public class BallParent : MonoBehaviour
	{
		[SerializeField] private Transform thisTransform;
		[SerializeField] private BallParentType type;

		public void Upgrade(float crossDistance, Vector2 sidesDistance)
		{
			switch (type)
			{
				case BallParentType.Upper:
					thisTransform.localPosition += new Vector3(0f, crossDistance, 0f);
					break;
				case BallParentType.RightUpper:
					thisTransform.localPosition += new Vector3(sidesDistance.x, sidesDistance.y, 0f);
					break;
				case BallParentType.Right:
					thisTransform.localPosition += new Vector3(crossDistance, 0f, 0f);
					break;
				case BallParentType.RightLower:
					thisTransform.localPosition += new Vector3(sidesDistance.x, -sidesDistance.y, 0f);
					break;
				case BallParentType.Lower:
					thisTransform.localPosition += new Vector3(0f, -crossDistance, 0f);
					break;
				case BallParentType.LeftLower:
					thisTransform.localPosition += new Vector3(-sidesDistance.x, -sidesDistance.y, 0f);
					break;
				case BallParentType.Left:
					thisTransform.localPosition += new Vector3(-crossDistance, 0f, 0f);
					break;
				case BallParentType.LeftUpper:
					thisTransform.localPosition += new Vector3(-sidesDistance.x, sidesDistance.y, 0f);
					break;
			}
		}
	}
}