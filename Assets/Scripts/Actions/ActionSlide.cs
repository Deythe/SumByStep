
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class DataActionSlide
{
    public Vector2 savedDirectionRequirement;
    
    public DataActionSlide(Vector2 directionRequirement_)
    {
        savedDirectionRequirement = directionRequirement_;
    }
}

public class ActionSlide : CustomAction
{
    [SerializeField] private Transform _transformCube;
    [SerializeField, HideInInspector] private Vector2 _directionRequirement;
    
    public override bool CheckCanExecuteAction()
    {
        _eventForRequirement?.Invoke(this);
        
        CheckDiagonals(ref _directionRequirement);
        
        Vector3 checkNextPosition = _transformCube.position + new Vector3(-_directionRequirement.y, 0, _directionRequirement.x);

        return TilesManager.Instance.HaveTileHere(new Vector2(checkNextPosition .x, checkNextPosition.z));
    }
    public override void ExecuteAction()
    { 
        Move(_directionRequirement);
    }
    public override void CancelAction()
    {
        Move(-_directionRequirement);
    }
    public override string SaveAction()
    {
        string json = JsonUtility.ToJson(new DataActionSlide(_directionRequirement));
        return json;
    }
    public override void LoadAction(string datas_)
    {
        DataActionSlide loadedData = JsonUtility.FromJson<DataActionSlide>(datas_);
        _directionRequirement = loadedData.savedDirectionRequirement;
    }
    public override void GetDatasForAction<T>(T param)
    {
        if (param is Vector2 pivotVector)
        {
            _directionRequirement = pivotVector;
        }
        else
        {
            Debug.LogWarning("Invalid param for ActionSlide");
        }
    }
    private void Move(Vector2 direction)
    {
        Vector3 nextPosition = _transformCube.position + new Vector3(-direction.y, 0, direction.x);

        TilesManager.Instance.UpdateTile(new Vector2(_transformCube.position.x, _transformCube.position.z), null);

        
        DOTween.Sequence()
            .Join(_transformCube.DOMove(nextPosition, _currentTimeDuration))
            .OnComplete(() =>
            {
                TilesManager.Instance.UpdateTile(new Vector2(_transformCube.position.x, _transformCube.position.z), _transformCube);
                notifyFinished?.Invoke();
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
