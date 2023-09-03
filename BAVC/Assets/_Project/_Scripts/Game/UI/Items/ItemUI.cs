using Game.Items;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image image;
        [SerializeField] private Image background;

        private ItemSO _itemSO;
        
        public void ConfigureItemUI(ItemSO itemSO)
        {
            _itemSO = itemSO;
            
            title.text = itemSO.name;
            title.color = itemSO.titleColor;

            description.text = itemSO.description;
            description.color = itemSO.descriptionColor;

            image.sprite = itemSO.itemSprite;
            image.color = itemSO.titleColor;

            background.color = itemSO.backgroundColor;
        }

        public void Choose()
        {
            _itemSO.UseItem();
            TimeManager.Instance.Resume();
            ItemsManager.Instance.CloseItemsPanel();
        }
    }
}