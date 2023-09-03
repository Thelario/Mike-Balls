using Game.Managers;
using Game.UI;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public void StartGame()
        {
            CurrencyManager.Instance.ConfigureCurrency();
            PartyManager.Instance.ConfigureCurrentParty();
            PlayerStats.Instance.ConfigurePlayerStats();
            PlayerMovement.Instance.ConfigurePlayerMovement();
            SpawnerManager.Instance.ConfigureSpawner();
            XpManager.Instance.ConfigureXp();
            Timer.Instance.ResetTimer();
            ShopManagerUI.Instance.ConfigureShopManagerUI();
            CanvasManager.Instance.SwitchCanvas(CanvasType.GameShopMenu, .5f);
        }
    }
}