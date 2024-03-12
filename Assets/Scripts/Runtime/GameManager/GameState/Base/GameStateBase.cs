namespace FS
{
    public abstract class GameStateBase
    {
        protected GameManager _manager;

        public abstract GameState GameStateType { get; }
        public abstract void OnEnter();
        public abstract void OnExit();

        public GameStateBase(GameManager manager)
        {
            this._manager = manager;
        }
        protected void RegisterCharacterCallbackEvent(CharacterDetailUI detailUI, Character character)
        {
            detailUI.SetCharacter(character);
            character.SetCallbackOnStatusUpdate((status) => detailUI.UpdateStatusUI(status));
        }

    }
}
