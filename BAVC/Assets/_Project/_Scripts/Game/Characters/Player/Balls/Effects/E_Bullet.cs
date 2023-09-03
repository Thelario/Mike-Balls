using Game.Managers;
using UnityEngine;

namespace Game
{
    public class E_Bullet : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private Transform thisTransform;
        [SerializeField] private SpriteRenderer bulletRenderer;
        [SerializeField] private bool destroyBulletOnTriggerEnter;

        private float _currentDamage;
        private float _currentRange;
        private float _currentSpeed;
        private Vector2 _startingPosition;
        private Color _bulletColor;

        public void ConfigureBullet(float damage, float range, float speed, Color color)
        {
            _currentDamage = damage;
            _currentRange = range;
            _currentSpeed = speed;
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
            ParticlesManager.Instance.CreateParticle(ParticleType.BulletDestroyedParticles, position, _bulletColor);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable ida))
                return;

            ida.TakeDamage(_currentDamage);
            
            SoundManager.Instance.PlaySound(SoundType.Hit);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectSmall, other.ClosestPoint(thisTransform.position), _bulletColor);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, _bulletColor);
            
            if (destroyBulletOnTriggerEnter)
                DestroyBullet(thisTransform.position);
        }
    }
}