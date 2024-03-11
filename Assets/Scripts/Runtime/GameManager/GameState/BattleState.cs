using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class BattleState : GameStateBase
    {
        private Coroutine _battlePhaseCoroutine;
        private BoardData _boardData;

        private IDamagable _player;
        private IDamagable _enemy;
        public BattleState(GameManager manager) : base(manager)
        {
            this._boardData = manager.BoardManager.BoardData;
        }

        public override GameState GameStateType => GameState.BATTLE;

        public bool IsBattleEnd
        {
            get
            {
                if (_player == null || _enemy == null)
                    return true;
                if (_player.IsDead || _enemy.IsDead)
                    return true;
                return false;
            }
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter BattleState");
            _player = _manager.PlayerSnake.Head;
            _enemy = DataManager.Instance.CurrentEnemy;

            if (_battlePhaseCoroutine != null)
                _manager.StopCoroutine(_battlePhaseCoroutine);
            _battlePhaseCoroutine = _manager.StartCoroutine(PlayBattleLoop());
        }

        public override void OnExit()
        {
            Debug.Log("OnExit BattleState");
            if (_battlePhaseCoroutine != null)
                _manager.StopCoroutine(_battlePhaseCoroutine);
        }
        private IEnumerator PlayBattleLoop()
        {
            while (IsBattleEnd == false)
            {
                yield return new WaitForSeconds(1);

                // player attack
                _enemy.TakeDamage(_player.GetDamageData());
                // enemey attack.
                _player.TakeDamage(_enemy.GetDamageData());

            }

            //reslove battle.
            ResloveBattle();
        }

        private void ResloveBattle()
        {
            if (_player.IsDead)
            {
                if (_manager.PlayerSnake.Count == 1)
                {
                    _manager.ChangeState(GameState.RESULT);
                }
                else
                {
                    Direction dir = _manager.PlayerSnake.Head.CurrentDirection;
                    _manager.PlayerSnake.RemoveHead();
                    _manager.PlayerSnake.MoveToNewSlot(dir);
                }
            }
            else
            {
                RemoveEnemy();

                _manager.ChangeState(GameState.NORMAL);
            }
        }

        private void RemoveEnemy()
        {
            SlotInfo currentSlot = this._boardData.GetSlotFromPosition(_enemy.CurrentPosition);
            currentSlot.Clear();
            //TODO: implement pooling.
            _enemy.gameObject.SetActive(false);
        }
    }
}