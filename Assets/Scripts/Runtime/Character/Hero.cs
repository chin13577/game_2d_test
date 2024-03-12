using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{

    public class Hero : Character, IInteractable
    {
        public GameObject headIcon;
        public static event Action OnHeroJoinTeam;
        public override Team Team => Team.PLAYER;

        public override void TakeDamage(DamageData damageData, IDamagable attacker)
        {
            this.Status.HP = Mathf.Clamp(this.Status.HP - damageData.Damage, 0, this.Status.TotalMaxHP);
            Debug.Log(attacker.gameObject.name + " attack " + gameObject.name + " " + damageData.Damage);

            base.InvokeOnStatusUpdateEvent(this.Status);
        }

        public void Interact(GameObject user)
        {
        }

        public void PostInteract(GameObject user)
        {
            // add to. player team.
            Hero hero = user.GetComponent<Hero>();
            if (hero != null)
            {
                GameManager.Instance.PlayerSnake.AddCharacter(this);
                OnHeroJoinTeam?.Invoke();
            }
        }

        public void SetActiveHeadIcon(bool isHead)
        {
            headIcon.SetActive(isHead);
        }

        protected override void ClearAllData()
        {
            base.ClearAllData();
            SetActiveHeadIcon(false);
        }

    }

}