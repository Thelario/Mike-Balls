using Game.Managers;
using UnityEngine;

namespace Game
{
	public class E_ExplosiveBullet : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Rigidbody2D rb2D;
		[SerializeField] private Transform thisTransform;
		[SerializeField] private SpriteRenderer bulletRenderer;
		[SerializeField] private GameObject explosionPrefab;

		private float _currentDamage;
		private float _currentRange;
		private float _currentSpeed;
		private float _currentDestroyTime;
		private Vector2 _startingPosition;
		private Color _bulletColor;

		public void ConfigureBullet(float damage, float range, float speed, Color color, float destroyTime)
		{
			_currentDamage = damage;
			_currentRange = range;
			_currentSpeed = speed;
			_currentDestroyTime = destroyTime;
			_bulletColor = color;
			bulletRenderer.color = color;
			_startingPosition = thisTransform.position;
		}

		private void Update()
		{
			CheckDistanceTravelled();
		}

		private void FixedUpdate()
		{
			rb2D.velocity = _currentSpeed * Time.fixedDeltaTime * thisTransform.right;
		}

		private void CheckDistanceTravelled()
		{
			Vector2 pos = thisTransform.position;
			Vector2 distanceTraveled = _startingPosition - new Vector2(pos.x, pos.y);

			if (Mathf.Abs(Mathf.Abs(distanceTraveled.magnitude)) >= _currentRange)
				DestroyBullet(pos);
		}
        
		private void DestroyBullet(Vector3 position)
		{
			Instantiate(explosionPrefab, position, Quaternion.identity).GetComponent<E_Explosion>()
				.ConfigureEffect(_currentDamage, _bulletColor, _currentDestroyTime);
			
			SoundManager.Instance.PlaySound(SoundType.Explosion);
			
			Destroy(gameObject);
		}
	}
}