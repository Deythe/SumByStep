using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Func<bool> notifyCameraMode;
    
    public Button cancelButton;
    
    [SerializeField] private UIDocument _gameUIDocument;
    private ProgressBar _lifeProgressBar;
    private Button _switchCameraModeButton;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        
        Instance = this;
        
        if (_gameUIDocument)
        {
            _lifeProgressBar = _gameUIDocument.rootVisualElement.Q<ProgressBar>("LifeProgressBar");
            cancelButton = _gameUIDocument.rootVisualElement.Q<Button>("CancelButton");
            _switchCameraModeButton = _gameUIDocument.rootVisualElement.Q<Button>("FreeCameraButton");
            
            _switchCameraModeButton.clicked += SwitchCameraMode;
        }
    }

    public void UpdateLifePointUI(int newLifePoint_, int maxLifePoint_)
    {
        _lifeProgressBar.highValue = maxLifePoint_;
        _lifeProgressBar.value = newLifePoint_;
        _lifeProgressBar.title = "Life Point: " + newLifePoint_;
    }

    private void SwitchCameraMode()
    {
        bool resultNotifyCameraMode = notifyCameraMode.Invoke();
        _switchCameraModeButton.text = resultNotifyCameraMode ? "On" : "Off";
    }
}
