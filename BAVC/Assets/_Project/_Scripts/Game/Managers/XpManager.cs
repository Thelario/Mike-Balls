using System.Collections;
using Game.Managers;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class XpManager : Singleton<XpManager>
    {
        public delegate void PlayerLevelUp();
        public static PlayerLevelUp OnLevelUp;
        
        [Header("Xp UI")]
        [SerializeField] private GameObject xpSliderGameObject;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private TMP_Text lvlText;

        [Header("Enemy Xp Animation")]
        [SerializeField] private Vector3 sliderDefaultScale;
        [SerializeField] private Vector3 sliderMaxScaleAnimated;
        [SerializeField] private float timeToMaxScale;
        [SerializeField] private float timeToDefaultScale;
        
        private int _currentXp;
        private int _maxXp;
        private int _currentLevel;
        private int _levelRate;

        public int CurrentLevel => _currentLevel;

        public void ConfigureXp()
        {
            _levelRate = 0;
            _currentXp = 0;
            _maxXp = 5;
            _currentLevel = 1;
            
            UpdateXpUI();
        }

        public void UpgradeXp()
        {
            _levelRate++;
        }

        public void UpdateCurrentXp(int xp)
        {
            _currentXp += xp + _levelRate;
            
            PopUp();
            
            xpSlider.value = _currentXp;
            
            if (_currentXp >= _maxXp)
                LevelUp();
        }
        
        private void LevelUp()
        {
            _currentLevel++;
            if (_currentLevel < 11)
                _maxXp = Mathf.FloorToInt(_maxXp * 1.7f);

            if (_currentLevel % 5 == 0)
            {
                PartyManager.Instance.UpgradeMaxPartyLimit();
                SpawnerManager.Instance.SpawnBoss(_currentLevel / 5 - 1);
            }
            
            _currentXp = 0;
            SoundManager.Instance.PlaySound(SoundType.LevelUp);
            OnLevelUp?.Invoke();
            
            UpdateXpUI();
            StartCoroutine(Co_WaitBeforeMenuChange());
        }

        private IEnumerator Co_WaitBeforeMenuChange()
        {
            TransitionsManager.Instance.Transition();
            
            yield return new WaitForSeconds(.5f);
            
            CurrencyManager.Instance.LevelUp();
            CurrencyManager.Instance.AddMoney();
            CanvasManager.Instance.SwitchCanvas(CanvasType.GameShopMenu);
            TimeManager.Instance.Pause();
            SpawnerManager.Instance.LevelUp();
        }

        private void UpdateXpUI()
        {
            lvlText.text = "" + _currentLevel;
            xpSlider.maxValue = _maxXp;
            xpSlider.value = _currentXp;
        }

        private void PopUp()
        {
            LeanTween.scale(xpSliderGameObject, sliderMaxScaleAnimated, timeToMaxScale).setOnComplete(PopDown);
        }

        private void PopDown()
        {
            LeanTween.scale(xpSliderGameObject, sliderDefaultScale, timeToDefaultScale);
        }
    }
}