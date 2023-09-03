using UnityEngine;

namespace Game
{
	public class LimitArea : MonoBehaviour
	{
		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				PlayerStats.Instance.TakeDamage(200);
			}
		}
	}
}