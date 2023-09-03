using System.Collections;
using Game.Managers;
using UnityEngine;

namespace Game
{
	public class E_ExpansiveWave : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform thisTransform;

		[Header("PopUp Animation Fields")]
		[SerializeField] private float popUpTime;
		
		private float _currentDamage;
		private Vector3 _currentMaxScale;
		private Color _ballColor;

		public void ConfigureEffect(float damage, float destroyTime, Vector3 scale, Color ballColor)
		{
			_currentDamage = damage;
			_currentMaxScale = scale;
			_ballColor = ballColor;
			thisTransform.localScale = Vector3.zero;
			PopUp();
			StartCoroutine(DestroyYourself(destroyTime));
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

			ida.TakeDamage(_currentDamage);
			
			SoundManager.Instance.PlaySound(SoundType.Damage, .4f);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitEffectMedium, other.ClosestPoint(thisTransform.position), _ballColor);
			ParticlesManager.Instance.CreateParticle(ParticleType.EnemyHitParticles, other.transform.position, _ballColor);
		}
		
		#region Animation

		private void PopUp()
		{
			LeanTween.scale(gameObject, _currentMaxScale, popUpTime);
		}

		private void PopDown()
		{
			LeanTween.scale(gameObject, Vector3.zero, popUpTime);
		}

		#endregion
	}
}