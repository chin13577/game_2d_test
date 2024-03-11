
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
        }

        public override void OnExit()
        {
            Debug.Log("OnExit ResultState");
        }
    }
}