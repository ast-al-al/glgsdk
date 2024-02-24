using System;
using System.Text;
using UnityEngine;

public enum EntityKind
{
    Unit,
    Upgrade
}

public enum Owner
{
    Player,
    Enemy
}

public class EntityData : MonoBehaviour
{
    public string universalKey;
    public EntityKind entityKind;
    public Owner owner;

    private static StringBuilder _sb = new StringBuilder();
    
    public string GetKey(bool useEntityKind, bool useOwner, string prefix = null, string suffix = null)
    {
        return GetKey(this, useEntityKind, useOwner, prefix, suffix);
    }

    public static string GetKey
    (
        EntityData entityData,
        bool entityKind,
        bool owner,
        string prefix = null,
        string suffix = null
    )
    {
        _sb.Clear();
        if (prefix != null)
        {
            _sb.Append(prefix);
            _sb.Append('_');
        }

        if (entityKind)
        {
            _sb.Append(entityData.entityKind);
            _sb.Append('_');
        }

        if (owner)
        {
            _sb.Append(entityData.owner);
            _sb.Append('_');
        }

        _sb.Append(entityData.universalKey);
        if (suffix != null)
        {
            _sb.Append('_');
            _sb.Append(suffix);
        }

        return _sb.ToString();
    }

    public static EntityKind[] GetKinds()
    {
        return (EntityKind[])Enum.GetValues(typeof(EntityKind));
    }
}