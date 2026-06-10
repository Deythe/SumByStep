using UnityEngine;

public class BlockerController : MonoBehaviour
{
    [SerializeField] private ActionsBehaviour _actionsBehaviour;
    [SerializeField] private Vector2[] _directions;

    private int _indexDirection;
    
    public void BindActionsVector(CustomAction currentCustomAction_)
    {
        currentCustomAction_.GetDatasForAction(_directions[_indexDirection]);
        _indexDirection++;
        if (_indexDirection >= _directions.Length)
        {
            _indexDirection = 0;
        }
    }
}
