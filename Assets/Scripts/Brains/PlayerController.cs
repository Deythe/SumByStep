using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField][Range(0,100)] private float _touchThreshold;
    
    [SerializeField] private CameraBehaviour _kingCameraBehaviour;
    [SerializeField] private ActionsBehaviour _kingActionsBehaviour;
    [SerializeField] private LifeBehaviour _kingLifeBehaviour;
    
    private IA_MainGame _playerInput;
    private Vector2 _roundDirection;
    private bool _firstActionRealisable;
    private bool _canDoAction;
    private bool _isOnUI;
    
    private void Awake()
    { 
        _playerInput = new IA_MainGame();
        _playerInput.Enable();
        
       _playerInput.MainGame.Movement.performed += StartAction;
       _playerInput.MainGame.CancelAction.performed += CancelLastActionInput;
       _playerInput.MainGame.Touch.performed += CheckTouch;
       
       _playerInput.MainGame.CameraMovement.Disable();
    }

    private void Start()
    {
        _firstActionRealisable = true;
        _kingActionsBehaviour.notifyAllActionsAvailable += ResetCanStartActions;
        
        _kingLifeBehaviour.notifyDeath += Death;
        _kingLifeBehaviour.notifyLifeChanged += UIManager.Instance.UpdateLifePointUI;
        
        UIManager.Instance.notifyCameraMode += SwitchCameraInputMode;
        UIManager.Instance.cancelButton.clicked += CancelLastAction;
    }

    private void Update()
    {
        if (EventSystem.current)
        {
            _isOnUI = EventSystem.current.IsPointerOverGameObject();
        }

        if (_kingCameraBehaviour && _playerInput.MainGame.CameraMovement.IsPressed() && !_isOnUI)
        {
           InputMoveCameraFree();
        }
    }

    private void StartAction(InputAction.CallbackContext context_)
    {
        if(_isOnUI || _canDoAction) return;

        Vector2 touchDeltaPosition = context_.ReadValue<Vector2>();
        
        if(touchDeltaPosition.magnitude < _touchThreshold) return;
        
        touchDeltaPosition.Normalize();
        Vector2 pivotRoundDirection = new Vector2(
            Mathf.RoundToInt(touchDeltaPosition.x), 
            Mathf.RoundToInt(touchDeltaPosition.y));

        if (pivotRoundDirection == Vector2.zero || !_firstActionRealisable) return;

        _firstActionRealisable = false;
        _roundDirection = pivotRoundDirection;

        if (_kingActionsBehaviour.IsFirstActionsRealizable())
        {
            ActionsManager.Instance.ExecuteAllActions();
            _canDoAction=true;
        }
        else
        {
            _firstActionRealisable = true;
        }
    }
    
    private void CheckTouch(InputAction.CallbackContext context_)
    {
        float isTouchingScreen = context_.ReadValue<float>();
        if (isTouchingScreen == 0)
        {
            _canDoAction = false;
        }
    }

    private void CancelLastActionInput(InputAction.CallbackContext context_)
    {
        if (!context_.canceled)
        {
            CancelLastAction();
        }
    }

    private void InputMoveCameraFree()
    {
        if (_kingCameraBehaviour && !_isOnUI)
        {
            Vector2 pivotVector = _playerInput.MainGame.CameraMovement.ReadValue<Vector2>();
            float reference = Mathf.Min(Screen.width, Screen.height);
            _kingCameraBehaviour.ManualMove(pivotVector/reference);
        }
    }
    
    public void BindActionsVector(CustomAction currentCustomAction_)
    {
        currentCustomAction_.GetDatasForAction(_roundDirection);
    }

    private void CancelLastAction()
    {
        if (_firstActionRealisable)
        {
            bool succeed = ActionsManager.Instance.CancelAllActions();
            
            if (succeed)
            {
                _firstActionRealisable = false;
            }
        }
    }
    
    private void ResetCanStartActions()
    {
        _firstActionRealisable = true;
    }

    private bool SwitchCameraInputMode()
    {
        bool pivotCameraMode = _kingCameraBehaviour.SwitchCameraFreeMode();
        
        if (pivotCameraMode)
        {
            _playerInput.MainGame.Movement.Disable();
            _playerInput.MainGame.CameraMovement.Enable();
        }
        else
        {
            _playerInput.MainGame.Movement.Enable();
            _playerInput.MainGame.CameraMovement.Disable();
        }
        
        return pivotCameraMode;
    }
    
    private void Death()
    {
        SceneManager.LoadScene(0);
    }
}
