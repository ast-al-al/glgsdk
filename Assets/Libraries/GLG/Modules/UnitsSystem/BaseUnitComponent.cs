using UnityEngine;

public abstract class BaseUnitComponent : MonoBehaviour
{
    protected Unit _unit;
    public virtual void InitializeOnUnit(Unit unit)
    {
        _unit = unit;
    }
    
    public T Get<T>() where T : BaseUnitComponent
    {
        return _unit.Get<T>();
    }
}
