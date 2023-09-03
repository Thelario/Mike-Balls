using System.Collections;
using Game.Managers;
using UnityEngine;

namespace Game
{
	public class E_Explosion : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform thisTransform;

		[Header("PopUp Animation Fields")]
		[SerializeField] private float popUpTime;
		[SerializeField] private float popDownTime;
		[SerializeField] private Vector3 maxScale;
		[SerializeField] private Vector3 defaultScale;
		
		private float _currentDamage;
		private Color _ballColor;

		public void ConfigureEffect(float damage, Color ballColor, float destroyTime)
		{
			_currentDamage = damage;
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