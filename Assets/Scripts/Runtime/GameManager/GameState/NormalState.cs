namespace FS
{
    public class NormalState : GameStateBase
    {
        PlayerSnake _playerSnake;
        public NormalState(GameManager manager) : base(manager)
        {
            _playerSnake = manager.PlayerSnake;
        }

        public override GameState GameStateType => GameState.NORMAL;

        public override void OnEnter()
        {
            GameManager.OnUpdateTurn += GameManager_OnUpdateTurn;
            _playerSnake.SetOnUpdateHead((Character newHead) => { _manager.Camera.FollowTarget(newHead.transform); });
        }

        public override void OnExit()
        {
            GameManager.OnUpdateTurn -= GameManager_OnUpdateTurn;
        }

        private void GameManager_OnUpdateTurn()
        {
            Direction direction = _manager.playerInputDir;
            ExecuteResult result = _playerSnake.ExecuteAndMove(direction);
            if (result == ExecuteResult.HIT_ENEMY)
            {
                // goto battle state.
            }
            else if (result == ExecuteResult.HIT_OWN_SNAKE_PART)
            {
                // goto result state.
            }
            else if (result == ExecuteResult.HIT_WALL)
            {
                if (_playerSnake.Count == 1)
                {
                    //goto result state
                }
            }
        }

    }
}
