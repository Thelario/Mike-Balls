using Game.Items;
using Game.Managers;
using UnityEngine;

namespace Game
{
    public class ItemsManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform itemsParent;

        private void OnEnable()
        {
            CreateItems();
        }

        private void OnDisable()
        {
            DestroyItems();
        }

        private void CreateItems()
        {
            ItemSO[] items = ItemsManager.Instance.GetThreeRandomAndDifferentItems();
            
            for (int i = 0; i < 3; i++)
            {
                Instantiate(itemPrefab, itemsParent).GetComponent<ItemUI>().ConfigureItemUI(items[i]);
            }
        }

        private void DestroyItems()
        {
            if (itemsParent.childCount == 0)
                return;
            
            foreach (Transform t in itemsParent)
                Destroy(t.gameObject);
        }
    }
}