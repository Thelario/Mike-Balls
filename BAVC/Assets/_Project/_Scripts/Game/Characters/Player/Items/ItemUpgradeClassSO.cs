using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item/UpgradeClassItem", order = 2)]
    public class ItemUpgradeClassSO : ItemSO
    {
        [Header("Class Upgrade Fields")]
        [SerializeField] private BallClass ballClass;
        [SerializeField] private float multiplier;
        
        public override void UseItem()
        {
            PartyManager.Instance.UpgradeBalls(multiplier, ballClass);
        }
    }
}