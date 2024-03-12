using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class PrepareState : GameStateBase
    {
        private BoardManager _boardManager;
        private UIManager _uiManager;

        public PrepareState(GameManager manager) : base(manager)
        {
            _boardManager = manager.BoardManager;
            this._uiManager = manager.UIManager;
        }

        public override GameState GameStateType => GameState.PREPARE;

        public override void OnEnter()
        {
            _manager.SetTurn(0);

            _uiManager.HideAll();
            Debug.Log("OnEnter PrepareState");
            _boardManager.ClearData();
            _boardManager.GenerateObstacle(_manager.ObstacleRatio);

            List<SlotInfo> emptySlotList = _boardManager.BoardData.GetAllEmptySlots();
            emptySlotList.Shuffle();

            Character hero = RandomSpawnCharacter(emptySlotList, Team.PLAYER);
            //TODO: remove this line. it just for debug.
            hero.sprite.color = Color.green;
            hero.name = "head";
            _manager.PlayerSnake.AddCharacter(hero);

            _manager.Camera.FollowTarget(hero.transform);

            for (int i = 0; i < 5; i++)
            {
                RandomSpawnCharacter(emptySlotList, Team.PLAYER);
            }
            for (int i = 0; i < 5; i++)
            {
                RandomSpawnCharacter(emptySlotList, Team.ENEMY);
            }

            StartScreenUI startScreenUI = this._uiManager.StartScreenUI;
            startScreenUI.Show();
            startScreenUI.startBtn.SetCallback(() =>
            {
                startScreenUI.Hide();
                _manager.ChangeState(GameState.NORMAL);
            });
        }
        public override void OnExit()
        {
            Debug.Log("OnExit PrepareState");
        }
        private Character RandomSpawnCharacter(List<SlotInfo> emptySlotList, Team team)
        {
            SlotInfo slot = emptySlotList[0];
            emptySlotList.RemoveAt(0);

            Character prefab = team == Team.PLAYER ? _manager.heroPrefab : _manager.enemyPrefab;
            Character character = GameObject.Instantiate(prefab).GetComponent<Character>();
            character.Init();
            character.CurrentPosition = slot.WorldPos;
            slot.SetObject(character);

            return character;
        }

    }
}