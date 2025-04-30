using DG.Tweening;
using UnityEngine;

public class ActionCubeMovements : Action
{
    private Transform _transformCube;
    private int _transformCubeID;
    private Vector2 _savedDirection;
    private bool _isMoving;

    public override void DoAction<T>(ref T parameter)
    {
        Vector2 vectorParam =  (Vector2)(object)parameter;
        Move(vectorParam);
    }

    public override void CancelAction()
    {
        Move(-_savedDirection);
    }

    public override void Save()
    {
        ActionCubeMovements test = new ActionCubeMovements();
    }

    private void Move(Vector2 direction)
    {
        if(_isMoving) return;

        CheckDiagonals(ref direction);

        _savedDirection = direction;
        Vector3 axis = Vector3.Cross(Vector3.up, new Vector3(-direction.y, 0, direction.x));
        Vector3 nextPosition = _transformCube.position + new Vector3(-direction.y, 0, direction.x);

        if (!BoardManager.Instance.HaveTileHere(new Vector2(nextPosition.x, nextPosition.z))) return;
        
        Quaternion fromRotation = _transformCube.rotation;
        Quaternion toRotation = Quaternion.AngleAxis(90f, axis) * fromRotation;
        
        _isMoving = true;
        _transformCube.DORotateQuaternion(toRotation, 0.5f);
        _transformCube.DOMove(nextPosition, 0.5f).OnComplete(
            () =>
            {
                _isMoving = false;
                Save();
            });
    }
    private void CheckDiagonals(ref Vector2 direction)
    {
        if(!direction.x.Equals(0) && !direction.y.Equals(0))
        {
            if (direction.x.Equals(direction.y))
            {
                direction.x = 0;
            }else
            {
                direction.y = 0;
            }
        }
    }
}
