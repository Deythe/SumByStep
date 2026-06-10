using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private List<Transform> _listTargetTransform;

    [SerializeField] private Vector3 _offset;
    [SerializeField][Range(0,1)] private float _speedFocus;
    
    [SerializeField] private bool _isFreeCamera;
    [SerializeField] private float _speedManual;
    
    [SerializeField] private int _targetFPS;
    private Vector3 _targetPosition;
    
    private void Awake()
    {
        _isFreeCamera = false;
        Application.targetFrameRate = _targetFPS;
    }

    private void Update()
    {
        Application.targetFrameRate = _targetFPS;

        if (_cameraTransform)
        {
            if (!_isFreeCamera)
            { 
                FocusMove();
            }
        }
    }

    public void ManualMove(Vector2 dir_)
    {
        _targetPosition = _cameraTransform.position + (-dir_.x * _cameraTransform.right + -dir_.y * _cameraTransform.up) * _speedManual;
        _cameraTransform.position = _targetPosition;
    }

    private void FocusMove()
    {
        _targetPosition = CalculateBarycenterPoint();
        _targetPosition.x += _offset.x;
        _targetPosition.z += _offset.z;
        _targetPosition.y = _offset.y;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _targetPosition, _speedFocus);
    }

    public bool SwitchCameraFreeMode()
    {
        _isFreeCamera = !_isFreeCamera;
        return _isFreeCamera;
    }

    private Vector3 CalculateBarycenterPoint()
    {
        if (_listTargetTransform.Count>0)
        {
            Vector3 pivotVector = Vector3.zero;
            foreach (var pivotTransform in _listTargetTransform)
            {
                if (pivotTransform)
                {
                    pivotVector += pivotTransform.position;
                }
            }
            
            return pivotVector / _listTargetTransform.Count;
        }
        
        return Vector3.zero;
    }
}
