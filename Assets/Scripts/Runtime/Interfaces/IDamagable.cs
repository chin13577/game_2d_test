using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageData
{
    public int Damage;
    public bool IsCritical;

}
public interface IDamagable
{
    public GameObject gameObject { get; }
    public DamageData GetDamageData();
    public void TakeDamage(DamageData damageData, IDamagable attacker);
    public bool IsDead { get; }
    public Vector3 CurrentPosition { get; }
}
