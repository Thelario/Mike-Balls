using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 2)]
    public abstract class ItemSO : ScriptableObject
    {
        [Header("Fields")]
        public string name;
        public string description;
        public Sprite itemSprite;
        
        [Header("Colors")]
        public Color titleColor;
        public Color backgroundColor;
        public Color descriptionColor;

        public abstract void UseItem();
    }
}