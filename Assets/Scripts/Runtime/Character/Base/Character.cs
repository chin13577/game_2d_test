using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public abstract class Character : MonoBehaviour, ISlotInfo, IDamagable
    {
        private Action<Status> OnStatusUpdate;
        public Status Status;
        public Character Previous;
        public Character Next;

        public SpriteRenderer sprite;

        public abstract Team Team { get; }

        public Direction NextDirection { get; set; }

        public Direction CurrentDirection { get; set; }
        public Direction LastDirection { get; set; }

        public Vector3 LastPosition;
        public Vector3 CurrentPosition
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public bool IsDead { get => Status.IsDead; }

        public virtual void Init(Status characterStatus = null)
        {
            this.Status = characterStatus == null ? new Status() : characterStatus;
        }

        public void SetSprite(string spriteId)
        {
            //TODO: load sprite async.
        }

        public void SetCallbackOnStatusUpdate(Action<Status> callback)
        {
            this.OnStatusUpdate = callback;
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            OnStatusUpdate = null;
        }

        public void Move(Direction direction)
        {
            LastDirection = CurrentDirection;
            CurrentDirection = direction;
            CurrentPosition += direction.ToVector3();
        }

        public DamageData GetDamageData()
        {
            //TODO: handle critical.
            bool isCritical = false;
            DamageData damageData = new DamageData()
            {
                Damage = Status.TotalAtk,
                IsCritical = isCritical
            };
            return damageData;
        }

        public virtual void TakeDamage(DamageData damageData, IDamagable attacker)
        {
            this.Status.HP = Mathf.Clamp(this.Status.HP - damageData.Damage, 0, this.Status.TotalMaxHP);
            Debug.Log(attacker.gameObject.name + " attack " + gameObject.name + " " + damageData.Damage);

            OnStatusUpdate?.Invoke(this.Status);
        }

    }
}