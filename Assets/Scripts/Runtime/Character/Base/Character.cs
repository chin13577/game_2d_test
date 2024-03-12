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
        [HideInInspector] public Character Previous;
        [HideInInspector] public Character Next;

        public SpriteRenderer sprite;

        [SerializeField] private List<Sprite> spriteList = new List<Sprite>();

        public abstract Team Team { get; }

        public Direction NextDirection { get; set; }

        public Direction CurrentDirection { get; set; }
        public Direction LastDirection { get; set; }

        protected void InvokeOnStatusUpdateEvent(Status status)
        {
            OnStatusUpdate?.Invoke(status);
        }

        public Vector3 LastPosition;
        private PlayerSnake _owner;

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
            SetRandomSprite();
        }

        public void SetOwnerSnake(PlayerSnake owner)
        {
            this._owner = owner;
        }

        public void SetRandomSprite()
        {
            int index = UnityEngine.Random.Range(0, spriteList.Count);
            this.sprite.sprite = spriteList[index];
        }

        public void SetCallbackOnStatusUpdate(Action<Status> callback)
        {
            this.OnStatusUpdate = callback;
        }

        private void OnEnable()
        {
            DataManager.OnUpdateTurn += GameManager_OnUpdateTurn;
        }

        private void OnDisable()
        {
            ClearAllEventEmitter();
            this._owner = null;
            Previous = null;
            Next = null;
            DataManager.OnUpdateTurn -= GameManager_OnUpdateTurn;
            ClearAllData();
        }

        protected virtual void ClearAllData()
        {

        }

        public void ClearAllEventEmitter()
        {
            OnStatusUpdate = null;
        }

        private void GameManager_OnUpdateTurn(int currentTurn)
        {
            //TODO: set 0.5 as config.
            int expGain = this._owner == null ? 1 : Math.Ceiling(this._owner.Count * 0.5f).ToInt32();

            if (Status.EXP + expGain >= Status.MaxEXP)
            {
                //TODO: show level up.
            }
            Status.EXP += expGain;
            OnStatusUpdate?.Invoke(Status);
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

        public abstract void TakeDamage(DamageData damageData, IDamagable attacker);

    }
}