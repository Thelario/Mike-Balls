using System.Collections;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Game
{
    public class Enemy : MonoBehaviour, IDamageable, IFreezable, IPushable
    {
        [Header("References")]
        [SerializeField] protected Transform thisTransform;
        [SerializeField] protected Rigidbody2D rb2D;
        [SerializeField] protected SpriteRenderer spRenderer;
        [SerializeField] protected Slider healthSlider;
        [SerializeField] protected GameObject spriteGameObject;
        [SerializeField] protected CircleCollider2D circleCollider2D;

        [Header("Fields")]
        [SerializeField] protected int damage;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float timeBetweenHits;
        [SerializeField] protected float hitAnimationScaleMultiplier;
        [SerializeField] protected Vector3 defaultEnemyScale;
        [SerializeField] protected Color defaultEnemyColor;
        [SerializeField] protected Color hitEnemyColor;

        protected int _currentDamage;
        protected float _currentHealth;
        protected float _freezeTimeCounter;
        protected float _currentMoveSpeed;
        protected bool _freezed;
        protected Color _freezedColor;

        protected virtual void Start()
        {
            ConfigureEnemy();
        }

        protected virtual void ConfigureEnemy()
        {
            _currentDamage = damage;
            _currentMoveSpeed = moveSpeed;
            _freezed = false;

            int addition;
            if (XpManager.Instance.CurrentLevel % 4 == 0 && !(XpManager.Instance.CurrentLevel % 9 == 0))
                addition = Mathf.FloorToInt(maxHealth + XpManager.Instance.CurrentLevel / 4);
            else
                addition = Mathf.FloorToInt(maxHealth + (XpManager.Instance.CurrentLevel - 1) / 4);
                
            _currentHealth = addition;
            healthSlider.maxValue = _currentHealth;
            healthSlider.value = _currentHealth;
            
            if (circleCollider2D != null)
                circleCollider2D.enabled = true;
        }

        protected virtual void Update()
        {
            CheckFreezing();
            Move();
        }

        protected virtual void CheckFreezing()
        {
            if (!_freezed)
                return;
            
            _freezeTimeCounter -= Time.deltaTime;
            if (_freezeTimeCounter > 0f)
                return;

            _freezed = false;
            spRenderer.color = defaultEnemyColor;
        }

        protected virtual void Move()
        {
            if (_freezed)
                return;
            
            Vector3 direction = PlayerMovement.Instance.PlayerPosition - thisTransform.position;
            direction.z = 0f;
            thisTransform.position += _currentMoveSpeed * Time.deltaTime * direction.normalized;
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            
            healthSlider.value = _currentHealth;
            StartCoroutine(Co_Hit());
            
            if (_currentHealth <= 0f)
                Die();
            else
                PopUp();
        }

        public void Freeze(float freezeTime, Color freezedColor)
        {
            rb2D.velocity = Vector2.zero;
            _freezeTimeCounter = freezeTime;
            spRenderer.color = freezedColor;
            _freezed = true;
            _freezedColor = freezedColor;
        }
        
        public void Push(float strength, float time)
        {
            StartCoroutine(Co_Push(strength, time));
        }

        protected virtual IEnumerator Co_Push(float strength, float time)
        {
            _currentMoveSpeed *= -strength;
            
            if (circleCollider2D != null)
                circleCollider2D.enabled = false;

            yield return new WaitForSeconds(time);

            _currentMoveSpeed /= -strength;
            
            if (circleCollider2D != null)
                circleCollider2D.enabled = true;
        }

        protected virtual IEnumerator Co_Hit()
        {
            spRenderer.color = hitEnemyColor;

            yield return new WaitForSeconds(timeBetweenHits);

            spRenderer.color = _freezed ? _freezedColor : defaultEnemyColor;
        }
        
        protected virtual void Die()
        {
            XpManager.Instance.UpdateCurrentXp(Random.Range(1, 3));
            ParticlesManager.Instance.CreateParticle(ParticleType.EnemyDeathParticles, thisTransform.position, defaultEnemyColor);
            Destroy(gameObject);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            PlayerStats.Instance.TakeDamage(_currentDamage);
            
            Die();
        }

        #region Animation
        
        private void PopUp()
        {
            LeanTween.scale(spriteGameObject, defaultEnemyScale * hitAnimationScaleMultiplier, timeBetweenHits / 2f).setOnComplete(PopDown);
        }

        private void PopDown()
        {
            LeanTween.scale(spriteGameObject, defaultEnemyScale, timeBetweenHits / 2f);
        }
        
        #endregion
    }
}