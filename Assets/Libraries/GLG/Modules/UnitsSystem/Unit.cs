using UnityEngine;

public class Unit : MonoBehaviour, IManagedAwake, IEntity
{
    [SerializeField] private EntityData _entityData;
    [SerializeField] private Transform _cachedTransform;
    [SerializeField] private BaseUnitComponent[] _unitComponents;

    public EntityData EntityData => _entityData;
    public Transform CachedTransform => _cachedTransform;

    public void ManagedAwake()
    {
        foreach (var component in _unitComponents) 
        {
            component.InitializeOnUnit(this);
        }
    }

    public T Get<T>() where T : BaseUnitComponent
    {
        for (int i = 0; i < _unitComponents.Length; i++)
        {
            if (_unitComponents[i] is T) return _unitComponents[i] as T;
        }
        return default;
    }
}
