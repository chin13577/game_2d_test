using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FS
{
    public class NormalState : GameStateBase
    {
        PlayerSnake _playerSnake;
        private PlayerInputController _playerInput;
        private GameUI _gameUI;
        float inputTimestamp = 0;
        public NormalState(GameManager manager) : base(manager)
        {
            _playerSnake = manager.PlayerSnake;
            this._playerInput = PlayerInputController.Instance;
            this._gameUI = manager.UIManager.GameUI;
        }

        public override GameState GameStateType => GameState.NORMAL;

        public override void OnEnter()
        {
            Debug.Log("OnEnter NormalState");
            _gameUI.Show();
            _gameUI.SetTurnText(DataManager.Instance.GetTurn());
            _gameUI.PlayerDetailUI.Show();
            base.RegisterCharacterCallbackEvent(_gameUI.PlayerDetailUI, _playerSnake.Head);

            _playerSnake.SetOnUpdateHead(OnPlayerSnakeUpdateHead);
            RegisterEvent();
        }

        public override void OnExit()
        {
            Debug.Log("OnExit NormalState");
            UnRegisterEvent();
        }
        #region Event

        private void RegisterEvent()
        {
            DataManager.OnUpdateTurn += OnManagerUpdateTurn;
            RegisterPlayerInputEvent();
        }

        private void UnRegisterEvent()
        {
            DataManager.OnUpdateTurn -= OnManagerUpdateTurn;
            UnRegisterPlayerInputEvent();
        }

        private void OnManagerUpdateTurn(int turn)
        {
            this._gameUI.SetTurnText(turn);
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
        #endregion

        #region PlayerInput

        private void MoveAction_performed(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Direction? inputDirection = input.ToDirection();
            if (inputDirection != null)
            {
                bool isMoveSuccess = OnPlayerUpdateInputDirection(inputDirection.Value);
                if (isMoveSuccess)
                {
                    _manager.OnPlayerMove();
                }
            }

        }
        private void RotateLeftAction_performed(InputAction.CallbackContext context)
        {
            _playerSnake.SwapCharacter(PlayerSnake.ESwapType.BACKWARD);
        }

        private void RotateRightAction_performed(InputAction.CallbackContext context)
        {
            _playerSnake.SwapCharacter(PlayerSnake.ESwapType.FORWARD);
        }

        #endregion

        private void OnPlayerSnakeUpdateHead(Character newHead)
        {
            Transform followTarget = newHead == null ? null : newHead.transform;
            _manager.Camera.FollowTarget(followTarget);

            _playerSnake.ClearAllCharacterEventEmitter();
            if (newHead != null)
            {
                base.RegisterCharacterCallbackEvent(_gameUI.PlayerDetailUI, _playerSnake.Head);
            }
        }
        private bool OnPlayerUpdateInputDirection(Direction direction)
        {
            if (Time.time - inputTimestamp < 0.1f)
            {
                return false;
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
            else if (result == ExecuteResult.PASS)
            {
                return true;
            }
            return false;
        }

    }
}
