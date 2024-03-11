using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class PrepareState : GameStateBase
    {
        public PrepareState(GameManager manager) : base(manager)
        {
        }

        public override GameState GameStateType => GameState.PREPARE;

        public override void OnEnter()
        {
            Debug.Log("OnEnter PrepareState");
        }

        public override void OnExit()
        {
            Debug.Log("OnExit PrepareState");
        }
    }
}