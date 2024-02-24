using UnityEngine;

public class UnitHealth : BaseUnitComponent, IDamageable
{
    public event System.Action<IDamageable, int> OnHpChanged;

    [SerializeField] protected int _maxHP;
    [SerializeField] protected int _startHP;

    protected event System.Action<IDamageable> _onDeath;

    public int CurrentHP { get; private set; }
    public int MaxHP => _maxHP;
    public EntityData EntityData => _unit.EntityData;

    public override void InitializeOnUnit(Unit unit)
    {
        base.InitializeOnUnit(unit);
        CurrentHP = _startHP;
    }

    public virtual void TakeDamage(int damage, IAttacker sender)
    {
        CurrentHP -= damage;
        int delta = CurrentHP;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            _onDeath?.Invoke(this);
            delta = -delta;
            OnHpChanged?.Invoke(this, delta);
            return;
        }
        OnHpChanged?.Invoke(this, -delta);
    }
    public virtual void Heal(int value)
    {
        int delta = CurrentHP;
        CurrentHP += value;
        if(CurrentHP > _maxHP)
        {
            CurrentHP = _maxHP;
            delta = _maxHP - delta;
            OnHpChanged?.Invoke(this, delta);
            return;
        }
        OnHpChanged?.Invoke(this, value);
    }
    public virtual void OnDeath(System.Action<IDamageable> callback)
    {
        _onDeath += callback;
    }
}
