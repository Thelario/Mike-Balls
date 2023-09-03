using Game.Managers;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class CameraFollow : Singleton<CameraFollow>
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform thisCameraTransform;

        [Header("Fields")]
        [SerializeField] private float smoothTime = 0.25f;
        [SerializeField] private float screenShakeTime = 0.2f;
        [SerializeField] private float cameraMaxMoveLimit = 0.05f;

        private float _screenShakeTimeCounter;
        private Vector3 _velocity = Vector3.zero;
        private readonly Vector3 _offset = new (0f, 0f, -10f);
        private bool _screenshakeIsHappening;

        private Camera _camera;

        public Vector3 CameraPosition => thisCameraTransform.position;

        protected override void Awake()
        {
            base.Awake();

            _camera = Camera.main;
            _screenshakeIsHappening = false;
        }

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            CalculateCamPos();
        }

        private void CalculateCamPos()
        {
            Vector3 targetPosition = player.position + _offset;
            thisCameraTransform.position =
                Vector3.SmoothDamp(thisCameraTransform.position, targetPosition, ref _velocity, smoothTime);
        }

        private IEnumerator Co_ScreenShake()
        {
            _screenShakeTimeCounter = screenShakeTime;

            while (_screenShakeTimeCounter > 0f)
            {
                float newRandomX = Random.Range(-cameraMaxMoveLimit, cameraMaxMoveLimit);
                float newRandomY = Random.Range(-cameraMaxMoveLimit, cameraMaxMoveLimit);

                transform.position += new Vector3(newRandomX, newRandomY, _offset.z);
                _screenShakeTimeCounter -= Time.deltaTime;

                yield return null;
            }
        }

        public void StopScreenShake()
        {
            _screenShakeTimeCounter = 0;
        }

        public void ScreenShake()
        {
            if (_screenShakeTimeCounter > 0f)
                return;
            
            StartCoroutine(nameof(Co_ScreenShake));
        }

        public void SetCameraShakeValue(float val)
        {
            screenShakeTime = val;
        }
    }
}