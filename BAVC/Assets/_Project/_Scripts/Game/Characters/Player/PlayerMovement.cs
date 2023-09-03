using Game.Managers;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : Singleton<PlayerMovement>
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private BallRotation ballRotation;
        
        public Vector3 PlayerPosition { get => playerTransform.position; }
        
        private float _horizontal;
        private float _vertical;

        public void ConfigurePlayerMovement()
        {
            playerTransform.position = Vector3.zero;
        }
        
        private void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (TimeManager.Instance.IsTimePause())
                    return;
                
                CanvasManager.Instance.SwitchCanvas(CanvasType.GamePauseMenu);
                TimeManager.Instance.Pause();
                CameraFollow.Instance.StopScreenShake();
            }
        }

        private void FixedUpdate()
        {
            rb2D.velocity = PlayerStats.Instance.MoveSpeed * Time.fixedDeltaTime * new Vector3(_horizontal, _vertical).normalized;
        }
    }
}
