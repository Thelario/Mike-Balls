using Game.Managers;
using UnityEngine;

namespace Game
{
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        private int _currentMoney = 4;
        private int _moneyRate = 2;
        private int _moneyRateUpgradable = 0;
        
        public int Money => _currentMoney;

        public void ConfigureCurrency()
        {
            _moneyRateUpgradable = 0;
            _currentMoney = 4;
            _moneyRate = 2;
        }

        public void Upgrade()
        {
            _moneyRateUpgradable++;
        }
        
        public void LevelUp()
        {
            _moneyRate = Mathf.Clamp(_moneyRate + 1, 1, 10) + Random.Range(_moneyRateUpgradable, _moneyRateUpgradable + 2);
        }
        
        public void AddMoney()
        {
            _currentMoney += _moneyRate;
        }

        public void SubstractMoney(int money)
        {
            _currentMoney = Mathf.Clamp(_currentMoney - money, 0, 9999999);
        }

        public bool CanPurchase(int cost)
        {
            return cost <= _currentMoney;
        }
    }
}