using GLG;
using UnityEngine;

public class FlyingHpBarCreator : MonoBehaviour, IManaged
{
    [SerializeField] private MonoBehaviour _iDamageableTarget;
    [SerializeField] private Vector3 _offset;
    private IDamageable _target;
    private void Awake()
    {
        Kernel.RegisterManaged(this);
        _target = _iDamageableTarget as IDamageable;
        Kernel.UI.Get<FlyingLabelsOverlay>().CreateHpBar(_iDamageableTarget as IDamageable, _offset);
    }
    
    private void OnDestroy()
    {
        Kernel.UnregisterManaged(this);
        if (_target != null)
        {
            Kernel.UI.Get<FlyingLabelsOverlay>().RemoveHpBar(_iDamageableTarget as IDamageable);
        }
    }

    public void ManagedUpdate()
    {
        if (_target.CurrentHP > 0) return;
        Kernel.UI.Get<FlyingLabelsOverlay>().RemoveHpBar(_iDamageableTarget as IDamageable);
        _target = null;
    }
}
