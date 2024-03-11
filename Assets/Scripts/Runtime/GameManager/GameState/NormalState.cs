using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FS
{
    public class NormalState : GameStateBase
    {
        PlayerSnake _playerSnake;
        private PlayerInputController _playerInput;
        float inputTimestamp = 0;
        public NormalState(GameManager manager) : base(manager)
        {
            _playerSnake = manager.PlayerSnake;
            this._playerInput = PlayerInputController.Instance;
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
            RegisterPlayerInputEvent();
        }

        public override void OnExit()
        {
            Debug.Log("OnExit NormalState");
            UnRegisterPlayerInputEvent();
        }

        private void RegisterPlayerInputEvent()
        {
            this._playerInput.MoveAction.performed += MoveAction_performed;
            this._playerInput.RotateLeftAction.performed += RotateLeftAction_performed;
            this._playerInput.RotateRightAction.performed += RotateRightAction_performed;
        }

        private void UnRegisterPlayerInputEvent()
        {
            this._playerInput.MoveAction.performed -= MoveAction_performed;
            this._playerInput.RotateLeftAction.performed -= RotateLeftAction_performed;
            this._playerInput.RotateRightAction.performed -= RotateRightAction_performed;
        }

        private void MoveAction_performed(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Direction? inputDirection = input.ToDirection();
            if (inputDirection != null)
            {
                OnPlayerUpdateInputDirection(inputDirection.Value);
            }
        }
        private void RotateLeftAction_performed(InputAction.CallbackContext context)
        {
            _playerSnake.SwapCharacter(PlayerSnake.ESwapType.FORWARD);
        }

        private void RotateRightAction_performed(InputAction.CallbackContext context)
        {
            _playerSnake.SwapCharacter(PlayerSnake.ESwapType.BACKWARD);
        }

        private void OnPlayerUpdateInputDirection(Direction direction)
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
