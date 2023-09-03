using System.Collections;
using Game.Items;
using UnityEngine;

namespace Game.Managers
{
    public class ItemsManager : Singleton<ItemsManager>
    {
        [SerializeField] private GameObject itemsPanel;

        [Space(10)]
        [SerializeField] private ItemSO[] items;

        public void OpenItemsPanel()
        {
            itemsPanel.SetActive(true);
            TimeManager.Instance.Pause();
        }

        public void OpenItemsPanel(float timeToWait)
        {
            StartCoroutine(nameof(Co_OpenItemsPanel), timeToWait);
        }

        private IEnumerator Co_OpenItemsPanel(float timeToWait)
        {
            yield return new WaitForSecondsRealtime(timeToWait);
            
            itemsPanel.SetActive(true);
            TimeManager.Instance.Pause();
        }

        public void CloseItemsPanel()
        {
            itemsPanel.SetActive(false);
            TimeManager.Instance.Resume();
        }

        public ItemSO[] GetThreeRandomAndDifferentItems()
        {
            ItemSO[] itemsResponse = new ItemSO[3];

            for (int i = 0; i < 3; i++)
            {
                ItemSO itemToAdd;
                do
                {
                    itemToAdd = items[Random.Range(0, items.Length)];
                }
                while (IsItemInList(itemsResponse, itemToAdd));

                itemsResponse[i] = itemToAdd;
            }

            return itemsResponse;
        }

        private bool IsItemInList(ItemSO[] itemsResponse, ItemSO itemToCheck)
        {
            foreach (ItemSO iso in itemsResponse)
            {
                if (iso != itemToCheck)
                    continue;

                return true;
            }

            return false;
        }
    }
}