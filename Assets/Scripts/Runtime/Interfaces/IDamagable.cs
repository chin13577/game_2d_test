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
    public void TakeDamage(DamageData damageData);
}
