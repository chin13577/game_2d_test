
using UnityEngine;

namespace FS
{
    public class ResultState : GameStateBase
    {
        public ResultState(GameManager manager) : base(manager)
        {
        }

        public override GameState GameStateType => GameState.RESULT;

        public override void OnEnter()
        {
            Debug.Log("OnEnter ResultState");
            ResultUI resultUI = _manager.UIManager.ResultUI;
            resultUI.Show();
            resultUI.resetBtn.SetCallback(() =>
            {
                resultUI.Hide();
                _manager.ChangeState(GameState.PREPARE);
            });
        }

        public override void OnExit()
        {
            Debug.Log("OnExit ResultState");
            ResultUI resultUI = _manager.UIManager.ResultUI;
            resultUI.Hide();
        }
    }
}