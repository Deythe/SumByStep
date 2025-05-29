using System;
using UnityEngine;

public class LifeBehaviour : MonoBehaviour
{
    public event Action notifyDeath;
    public event Action<int, int> notifyLifeChanged;
    
    [SerializeField] private int _maxLife;
    [SerializeField] private int _currentLife;

    private void Start()
    {
        _currentLife = _maxLife;
        notifyLifeChanged?.Invoke(_currentLife, _maxLife);
    }

    public void UpdateLifePoint(int updateLifePoint)
    {
        int pivotLife = _currentLife + updateLifePoint;
        _currentLife = Math.Clamp(pivotLife, 0, _maxLife);
        notifyLifeChanged?.Invoke(_currentLife, _maxLife);
        
        if (_currentLife <= 0)
        {
            notifyDeath?.Invoke();
        }
    }
}
