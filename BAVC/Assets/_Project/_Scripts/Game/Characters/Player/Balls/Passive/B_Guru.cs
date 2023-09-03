using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct GuruLevels
    {
        public float moveSpeed;
        public float rotationSpeed;
        public float crossDistance;
        public Vector2 sidesDistance;
    }
    
    public class B_Guru : Ball
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer ballRenderer;
        
        [Space(10)]
        [SerializeField] private GuruLevels[] guruLevelsArray;
        
        private void Start()
        {
            ConfigureBall();
        }
        
        public override void ConfigureBall()
        {
            _currentLevel = 0;
            ballRenderer.color = ballColor;
            Upgrade();
        }
        
        public override void Upgrade()
        {
            _currentLevel++;
            GuruLevels gl = guruLevelsArray[_currentLevel - 1];
            PlayerStats.Instance.Upgrade(gl.moveSpeed, gl.rotationSpeed, gl.crossDistance, gl.sidesDistance); 
        }
    }
}