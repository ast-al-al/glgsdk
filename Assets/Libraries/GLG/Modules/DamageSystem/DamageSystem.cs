using System;
using UnityEngine;

public class DamageSystem : MonoBehaviour, IManagedService
{
    public Type Type => typeof(DamageSystem);

    public void GetDamageMultiplier(IAttacker attacker, IDamageable damageable)
    {

    }
}
