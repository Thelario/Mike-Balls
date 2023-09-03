using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct ExecutionerLevels
    {
        public float damage;
        public Vector3 maxScale;
    }
    public class B_Executioner : Ball
    {
        [SerializeField] private GameObject expansiveWavePrefab;
        [SerializeField] private float expansiveWaveDestroyTime;
        [SerializeField] private ExecutionerLevels[] executionerLevels;

        private float _currentDamage;
        private Vector3 _currentMaxScale;
        private Color _ballColor;
        
        private void Start()
        {
            ConfigureBall();

            PlayerStats.PlayerHit += CreateWave;
        }

        private void OnDestroy()
        {
            PlayerStats.PlayerHit -= CreateWave;
        }

        public override void ConfigureBall()
        {
            _currentLevel = 0;
            _currentMultiplier = PartyManager.Instance.GetMultiplier(ballClass);
            _ballColor = ballColor;
            Upgrade();
        }
        
        public override void Upgrade()
        {
            _currentLevel++;
            _currentDamage = executionerLevels[_currentLevel - 1].damage * _currentMultiplier;
            _currentMaxScale = executionerLevels[_currentLevel - 1].maxScale;
        }
        
        public override void UpgradeObject(float multiplier)
        {
            _currentMultiplier = multiplier;
            _currentDamage = executionerLevels[_currentLevel - 1].damage * _currentMultiplier;
        }

        private void CreateWave()
        {
            Instantiate(expansiveWavePrefab, transform.position, Quaternion.identity)
                .GetComponent<E_ExpansiveWave>().ConfigureEffect(_currentDamage, expansiveWaveDestroyTime, _currentMaxScale, _ballColor);
        }
    }
}