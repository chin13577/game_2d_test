namespace FS
{
    public class NormalState : GameStateBase
    {
        public NormalState(GameManager manager) : base(manager)
        {

        }

        public override GameState GameStateType => GameState.NORMAL;

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }
    }
}
