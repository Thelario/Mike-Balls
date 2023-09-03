using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// CanvasSwitcher must be added to each button that will redirect to another panel (ex. MainMenu -> GameMenu through button Play).
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class CanvasSwitcher : MonoBehaviour
    {
        [SerializeField] private CanvasType desiredCanvasType;
        [SerializeField] private bool transition;

        private Button _menuButton;

        private void Start()
        {
            _menuButton = GetComponent<Button>();
            _menuButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            switch (desiredCanvasType)
            {
                case CanvasType.GameMenu:
                    SoundManager.Instance.PlaySound(SoundType.ButtonClicked);
                    if (transition)
                    {
                        TransitionsManager.Instance.Transition();
                        TimeManager.Instance.Resume(.5f);
                        CanvasManager.Instance.SwitchCanvas(desiredCanvasType, .5f);
                    }
                    else
                    {
                        TimeManager.Instance.Resume();
                        CanvasManager.Instance.SwitchCanvas(desiredCanvasType);
                    }
                    break;
                default:
                    SoundManager.Instance.PlaySound(SoundType.ButtonClicked);
                    if (transition)
                    {
                        TransitionsManager.Instance.Transition();
                        TimeManager.Instance.Pause(.5f);
                        CanvasManager.Instance.SwitchCanvas(desiredCanvasType, .5f);
                    }
                    else
                    {
                        TimeManager.Instance.Pause();
                        CanvasManager.Instance.SwitchCanvas(desiredCanvasType);
                    }
                    break;
            }
        }
    }
}
