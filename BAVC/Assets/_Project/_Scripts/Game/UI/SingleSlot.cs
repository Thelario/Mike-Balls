using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class SingleSlot : MonoBehaviour
	{
		[SerializeField] private Image slotImage;

		public void ConfigureSlot(Color color)
		{
			slotImage.color = color;
		}
	}
}