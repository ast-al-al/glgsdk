public interface IDamageable : IEntity
{
    public int CurrentHP { get; }
    public int MaxHP { get; }
    public void TakeDamage(int damage, IAttacker sender);
    public void OnDeath(System.Action<IDamageable> callback);
}