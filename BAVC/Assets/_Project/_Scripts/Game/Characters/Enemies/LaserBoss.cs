using Game.Managers;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class LaserBoss : Enemy
    {
        [Header("Stats")]
        [SerializeField] private float fireRate;
        [SerializeField] private float laserActiveTime;

        [Header("References")]
        [SerializeField] private GameObject[] lasers;
        [SerializeField] private E_Laser[] lasersSrc;

        private float _fireRateCounter;
        private float _laserActiveTimeCounter;
        private bool _laserIsActive;

        protected override void Update()
        {
        	base.Update();

            if (_laserIsActive)
            {
                _laserActiveTimeCounter -= Time.deltaTime;
                if (_laserActiveTimeCounter <= 0f)
                    DisableLasers();
            }
            else
            {
                _fireRateCounter -= Time.deltaTime;
                if (_fireRateCounter <= 0f)
                    EnableLasers();
            }
        }

        private void DisableLasers()
        {
            _fireRateCounter = fireRate;
            _laserActiveTimeCounter = laserActiveTime;
            _laserIsActive = false;

            foreach (GameObject laser in lasers)
                laser.SetActive(false);
        }

        private void EnableLasers()
        {
            _fireRateCounter = fireRate;
            _laserActiveTimeCounter = laserActiveTime;
            _laserIsActive = true;
            
            for (int i = 0; i < 3; i++)
            {
                lasers[i].SetActive(true);
                lasersSrc[i].ConfigureLaser(_currentDamage, defaultEnemyColor);
            }
        }

        protected override void Die()
        {
            SoundManager.Instance.PlaySound(SoundType.BossDeath);
        	ParticlesManager.Instance.CreateParticle(ParticleType.BossDeathParticles, thisTransform.position, defaultEnemyColor);
            ItemsManager.Instance.OpenItemsPanel(.75f);
            TransitionsManager.Instance.Transition();
            SoundManager.Instance.PlaySound(SoundType.Victory);
            TimeManager.Instance.Pause(.5f);
            CanvasManager.Instance.SwitchCanvas(CanvasType.GameVictoryMenu, .5f);
        	Destroy(gameObject);
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
        	if (!other.gameObject.CompareTag("Player"))
        		return;
            
        	PlayerStats.Instance.TakeDamage(damage);
        	SoundManager.Instance.PlaySound(SoundType.Damage);
        }
    }
}