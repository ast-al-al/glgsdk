using UnityEngine;

[System.Serializable]
public class EntitiesWhitelist
{
    [SerializeField] private EntityMatchingRule[] _rules;

    public bool IsEntityAllowed(EntityData entity)
    {
        for (int i = 0; i < _rules.Length; i++)
        {
            if (!_rules[i].IsEntityMatching(entity)) return false;
        }
        return true;
    }
    public bool IsTagAllowed(MonoBehaviour obj)
    {
        for (int i = 0; i < _rules.Length; i++)
        {
            if (!_rules[i].IsTagMatching(obj)) return false;
        }
        return true;
    }
    public bool IsTagAllowed(Component obj)
    {
        for (int i = 0; i < _rules.Length; i++)
        {
            if (!_rules[i].IsTagMatching(obj)) return false;
        }
        return true;
    }
}
