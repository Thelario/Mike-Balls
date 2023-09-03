using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game
{
	public class E_Fire : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform thisTransform;

		[Header("PopUp Animation Fields")]
		[SerializeField] private float popUpTime;
		[SerializeField] private float popDownTime;
		[SerializeField] private Vector3 maxScale;
		[SerializeField] private Vector3 defaultScale;

		private float _timeBetweenHits;
		private float _timeBetweenHitsCounter;
		private float _currentDamage;
		private Color _ballColor;

		private List<GameObject> enemies;

		private void Start()
		{
			enemies = new List<GameObject>();
		}

		public void ConfigureEffect(float damage, float timeBetweenHits, Color ballColor, float destroyTime)
		{
			_currentDamage = damage;
			_timeBetweenHits = timeBetweenHits;
			_timeBetweenHitsCounter = 0.05f;
			_ballColor = ballColor;
			thisTransform.localScale = Vector3.zero;
			PopUp();
			StartCoroutine(DestroyYourself(destroyTime));
		}

		private void Update()
		{
			_timeBetweenHitsCounter -= Time.deltaTime;
			if (_timeBetweenHitsCounter > 0f)
				return;

			_timeBetweenHitsCounter = _timeBetweenHits;
			DamageAllEnemiesWithinFire();
		}

		private void DamageAllEnemiesWithinFire()
		{
			if (enemies.Count <= 0)
				return;
			
			SoundManager.Instance.PlaySound(SoundType.FireEffect);
			for (int i = 0; i < enemies.Count; i++)
			{
				if (enemies[i] == null)
					continue;
				
				Vector3 effectsPos = enemies[i].transform.position;
				effectsPos = new Vector3(effectsPos.x + Random.Range(-0.1f, 0.1f), effectsPos.y + Random.Range(-0.1f, 0.1f), effectsPos.z);
				ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectSmall, effectsPos, _ballColor);
				ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, effectsPos, _ballColor);
				enemies[i].GetComponent<IDamageable>().TakeDamage(_currentDamage);
			}
		}

		private IEnumerator DestroyYourself(float destroyTime)
		{
			yield return new WaitForSeconds(destroyTime);
			
			PopDown();
			Destroy(gameObject, popUpTime);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent(out IDamageable ida))
				return;

			enemies.Add(other.gameObject);
			
			ida.TakeDamage(_currentDamage);
			
			SoundManager.Instance.PlaySound(SoundType.FireEffect);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectSmall, other.ClosestPoint(thisTransform.position), _ballColor);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, _ballColor);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag("Enemy"))
				return;

			enemies.Remove(other.gameObject);
		}

		#region Animation

		private void PopUp()
		{
			LeanTween.scale(gameObject, maxScale, popUpTime * 0.2f).setOnComplete(PopUpDefault);
		}

		private void PopUpDefault()
		{
			LeanTween.scale(gameObject, defaultScale, popUpTime * 0.8f);
		}

		private void PopDown()
		{
			LeanTween.scale(gameObject, maxScale, popDownTime * 0.2f).setOnComplete(PopDownDefault);
		}
		
		private void PopDownDefault()
		{
			LeanTween.scale(gameObject, Vector3.zero, popDownTime * 0.8f);
		}

		#endregion
	}
}