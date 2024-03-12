using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{

    public class Monster : Character
    {
        public static event Action OnMonsterDead;
        public override Team Team => Team.ENEMY;

        public override void TakeDamage(DamageData damageData, IDamagable attacker)
        {
            this.Status.HP = Mathf.Clamp(this.Status.HP - damageData.Damage, 0, this.Status.TotalMaxHP);
            Debug.Log(attacker.gameObject.name + " attack " + gameObject.name + " " + damageData.Damage);

            base.InvokeOnStatusUpdateEvent(this.Status);

            if (IsDead)
            {
                OnMonsterDead?.Invoke();
            }
        }

    }

}