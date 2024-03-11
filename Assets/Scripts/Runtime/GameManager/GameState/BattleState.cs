using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class BattleState : GameStateBase
    {
        public BattleState(GameManager manager) : base(manager)
        {
        }

        public override GameState GameStateType => GameState.BATTLE;

        public override void OnEnter()
        {
            Debug.Log("OnEnter BattleState");
        }

        public override void OnExit()
        {
            Debug.Log("OnExit BattleState");
        }
    }
}