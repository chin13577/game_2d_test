using UnityEngine;

namespace FS
{
    public class NormalState : GameStateBase
    {
        PlayerSnake _playerSnake;
        float inputTimestamp = 0;
        public NormalState(GameManager manager) : base(manager)
        {
            _playerSnake = manager.PlayerSnake;
        }

        public override GameState GameStateType => GameState.NORMAL;

        public override void OnEnter()
        {
            Debug.Log("OnEnter NormalState");
            _playerSnake.SetOnUpdateHead((Character newHead) =>
            {
                if (newHead != null)
                {
                    _manager.Camera.FollowTarget(newHead.transform);
                }
                else
                {
                    _manager.Camera.FollowTarget(null);
                }
            });
        }

        public override void OnExit()
        {
            Debug.Log("OnExit NormalState");
        }

        public override void OnPlayerUpdateInputDirection(Direction direction)
        {
            if (Time.time - inputTimestamp < 0.1f)
            {
                return;
            }
            inputTimestamp = Time.time;
            ExecuteResult result = _playerSnake.ExecuteAndMove(direction);
            if (result == ExecuteResult.HIT_ENEMY)
            {
                // goto battle state.
                SlotInfo nextSlot = _playerSnake.GetNextSlot(direction);
                Monster enemy = nextSlot.Obj.gameObject.GetComponent<Monster>();
                DataManager.Instance.SetCurrentBattleEnemy(enemy);
                _manager.ChangeState(GameState.BATTLE);
            }
            else if (result == ExecuteResult.HIT_OWN_SNAKE_PART)
            {
                // goto result state.
                _manager.ChangeState(GameState.RESULT);
            }
            else if (result == ExecuteResult.HIT_WALL)
            {
                if (_playerSnake.Count == 0)
                {
                    //goto result state
                    _manager.ChangeState(GameState.RESULT);
                }
            }
        }

    }
}
