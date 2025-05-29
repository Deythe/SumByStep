using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CameraBehaviour _kingCameraBehaviour;
    [SerializeField] private ActionsBehaviour _kingActionsBehaviour;
    [SerializeField] private LifeBehaviour _kingLifeBehaviour;
    
    private IA_MainGame _playerInput;
    private Vector2 _roundDirection;
    private bool _canStartActions;
    private bool isOnUI;

    private void Awake()
    { 
        _playerInput = new IA_MainGame();
        _playerInput.Enable();
        
       _playerInput.MainGame.Movement.performed += StartAction;
       _playerInput.MainGame.CancelAction.performed += CancelLastActionInput;
       _playerInput.MainGame.CameraMovement.performed += InputMoveCamera;
       
       _playerInput.MainGame.CameraMovement.Disable();
    }

    private void Start()
    {
        _canStartActions = true;
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
            isOnUI = EventSystem.current.IsPointerOverGameObject();
        }

        if (_kingCameraBehaviour && _playerInput.MainGame.CameraMovement.IsPressed() && !isOnUI)
        {
            Vector2 pivotVector = _playerInput.MainGame.CameraMovement.ReadValue<Vector2>();
            Vector2 pivotRoundDirection = new Vector2(
                Mathf.RoundToInt(pivotVector.x), 
                Mathf.RoundToInt(pivotVector.y));
            
            _kingCameraBehaviour.ManualMove(pivotRoundDirection);
        }
    }

    private void StartAction(InputAction.CallbackContext context_)
    {
        if(isOnUI) return;
        
        Vector2 pivotRoundDirection = new Vector2(
            Mathf.RoundToInt(context_.ReadValue<Vector2>().x), 
            Mathf.RoundToInt(context_.ReadValue<Vector2>().y));

        if (pivotRoundDirection == Vector2.zero || !_canStartActions) return;

        _canStartActions = false;
        _roundDirection = pivotRoundDirection;

        if (_kingActionsBehaviour.IsFirstActionsRealizable())
        {
            ActionsManager.Instance.ExecuteAllActions();
        }
        else
        {
            _canStartActions = true;
        }
    }

    private void CancelLastActionInput(InputAction.CallbackContext context_)
    {
        if (!context_.canceled)
        {
            CancelLastAction();
        }
    }

    private void InputMoveCamera(InputAction.CallbackContext context_)
    {
        if (_kingCameraBehaviour && !isOnUI)
        {
            _kingCameraBehaviour.ManualMove(context_.ReadValue<Vector2>());
        }
    }
    
    public void BindActionsVector(CustomAction currentCustomAction_)
    {
        currentCustomAction_.GetDatasForAction(_roundDirection);
    }

    private void CancelLastAction()
    {
        if (_canStartActions)
        {
            bool succeed = ActionsManager.Instance.CancelAllActions();
            
            if (succeed)
            {
                _canStartActions = false;
            }
        }
    }
    
    private void ResetCanStartActions()
    {
        _canStartActions = true;
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
