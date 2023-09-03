using System.Collections;
using Game.Managers;
using UnityEngine;

namespace Game
{
	public class E_Whirlwind : MonoBehaviour
	{
    	[Header("References")]
        [SerializeField] private Transform thisTransform;
        [SerializeField] private float timeToWaitBeforeDestroy;

        private float _currentDamage;
        private float _currentRange;
        private float _currentSpeed;
        private Vector2 _startingPosition;
        private Color _bulletColor;

        public void ConfigureWhirlwind(float damage, float range, float speed, Color color)
        {
            _currentDamage = damage;
            _currentRange = range;
            _currentSpeed = speed;
            _bulletColor = color;
            _startingPosition = thisTransform.position;
        }

        private void Update()
        {
            CheckDistanceTravelled();
            
            thisTransform.position += _currentSpeed * Time.deltaTime * thisTransform.right;
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
            _currentSpeed = 0f;
            LeanTween.scale(gameObject, Vector3.zero, timeToWaitBeforeDestroy);
            StartCoroutine(Co_WaitBeforeDestroy(position));
        }

        private IEnumerator Co_WaitBeforeDestroy(Vector3 position)
        {
            yield return new WaitForSeconds(timeToWaitBeforeDestroy);
            
            ParticlesManager.Instance.CreateParticle(ParticleType.BulletDestroyedParticles, position, _bulletColor);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable ida))
                return;

            ida.TakeDamage(_currentDamage);
            
            SoundManager.Instance.PlaySound(SoundType.Damage);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectSmall, other.ClosestPoint(thisTransform.position), _bulletColor);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, _bulletColor);
        }
	}
}