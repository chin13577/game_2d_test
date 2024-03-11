namespace FS
{
    public abstract class GameStateBase
    {
        protected GameManager _manager;

        public abstract GameState GameStateType { get; }
        public abstract void OnEnter();
        public abstract void OnExit();

        public virtual void OnPlayerUpdateInputDirection(Direction direction)
        {

        }

        public GameStateBase(GameManager manager)
        {
            this._manager = manager;
        }
    }
}
