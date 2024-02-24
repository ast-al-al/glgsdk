using UnityEngine;

[System.Serializable]
public class EntityMatchingRule
{
    [SerializeField] private bool _checkForTag;
    [SerializeField] private string _targetTag;
    [SerializeField] private bool _checkForOwner;
    [SerializeField] private Owner _targetOwner;
    [SerializeField] private bool _checkForEntityKind;
    [SerializeField] private EntityKind _targetEntityKind;
    [SerializeField] private bool _checkForUniversalKey;
    [SerializeField] private string _targetUniversalKey;

    public bool IsEntityMatching(EntityData entity)
    {
        if (_checkForOwner && entity.owner != _targetOwner) return false;
        if (_checkForEntityKind && entity.entityKind != _targetEntityKind) return false;
        if (_checkForUniversalKey && entity.universalKey != _targetUniversalKey) return false;
        return true;
    }
    public bool IsTagMatching(MonoBehaviour obj)
    {
        return !_checkForTag || obj.CompareTag(_targetTag);
    }
    public bool IsTagMatching(Component obj)
    {
        return !_checkForTag || obj.CompareTag(_targetTag);
    }
}
