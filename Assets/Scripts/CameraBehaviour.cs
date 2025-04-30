using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _targetTransform;

    [SerializeField] private Vector2 _offset;
    [SerializeField][Range(0,1)] private float _speedFocus;
    private void Update()
    {
        if (_targetTransform && _cameraTransform)
        {
            Vector3 targetPosition = new Vector3(_targetTransform.position.x+_offset.x, _cameraTransform.position.y , _targetTransform.position.z+_offset.y);
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetPosition, _speedFocus);
        }
    }
}
