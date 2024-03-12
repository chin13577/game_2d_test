using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class BattleState : GameStateBase
    {
        private Coroutine _battlePhaseCoroutine;
        private BoardData _boardData;
        private UIManager _uiManager;
        private GameUI _gameUI;
        private Character _player;
        private Character _enemy;
        public BattleState(GameManager manager) : base(manager)
        {
            this._boardData = manager.BoardManager.BoardData;
            this._uiManager = manager.UIManager;
            this._gameUI = this._uiManager.GameUI;
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

            _gameUI.Show();
            _gameUI.PlayerDetailUI.Show();
            _gameUI.EnemyDetailUI.Show();
            base.RegisterCharacterCallbackEvent(_gameUI.PlayerDetailUI, _player);
            base.RegisterCharacterCallbackEvent(_gameUI.EnemyDetailUI, _enemy);


            if (_battlePhaseCoroutine != null)
                _manager.StopCoroutine(_battlePhaseCoroutine);
            _battlePhaseCoroutine = _manager.StartCoroutine(PlayBattleLoop());
        }

        public override void OnExit()
        {
            Debug.Log("OnExit BattleState");
            if (_battlePhaseCoroutine != null)
                _manager.StopCoroutine(_battlePhaseCoroutine);

            _gameUI.EnemyDetailUI.Hide();
        }

        private IEnumerator PlayBattleLoop()
        {
            while (IsBattleEnd == false)
            {
                yield return new WaitForSeconds(1);

                // player attack
                DamageData playerDamage = _player.GetDamageData();
                _enemy.TakeDamage(playerDamage, _player);
                //TODO: spawn DamangeText.
                //if critical -> show damage critical.

                // enemey attack.
                DamageData enemyDamage = _enemy.GetDamageData();
                _player.TakeDamage(enemyDamage, _enemy);
                //TODO: spawn DamangeText.
                //if critical -> show damage critical.

            }

            ResloveBattle();
        }

        private void ResloveBattle()
        {
            if (_enemy.IsDead)
            {
                RemoveEnemy();
            }
            if (_player.IsDead)
            {
                if (_manager.PlayerSnake.Count == 1)
                {
                    _manager.ChangeState(GameState.RESULT);
                    return;
                }
                else
                {
                    Direction dir = _manager.PlayerSnake.Head.CurrentDirection;
                    _manager.PlayerSnake.RemoveHead();
                    _manager.PlayerSnake.MoveToNewSlot(dir);
                }
            }
            _manager.ChangeState(GameState.NORMAL);
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