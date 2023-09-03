using Game.Managers;
using UnityEngine;

namespace Game
{
    public class E_Sword : MonoBehaviour
    {
        private Color _ballColor;
        private float _currentDamage;
        private float _currentRange;
        private float _currentSwipeTime;
        
        public void ConfigureEffect(Color ballColor, float damage, float range, float swipeTime, bool rotate)
        {
            _ballColor = ballColor;
            _currentDamage = damage;
            _currentRange = range;
            _currentSwipeTime = swipeTime;
            
            gameObject.transform.localScale = Vector3.zero;
            
            if (rotate)
                gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-30f, 30f));

            MoveSwordLocally();
        }

        private void MoveSwordLocally()
        {
            LeanTween.moveLocalY(gameObject, _currentRange, _currentSwipeTime)
                .setEase(LeanTweenType.easeInOutCubic)
                .setLoopPingPong(1)
                .setOnComplete(DestroyYourself);

            LeanTween.scale(gameObject, Vector3.one, _currentSwipeTime)
                .setEase(LeanTweenType.easeInOutCubic)
                .setLoopPingPong(1);
        }

        private void DestroyYourself()
        {
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable ida))
                return;

            ida.TakeDamage(_currentDamage);
            SoundManager.Instance.PlaySound(SoundType.SwordSwipe);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectMedium, other.transform.position, _ballColor);
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, _ballColor);
            CameraFollow.Instance.ScreenShake();
        }
    }
}