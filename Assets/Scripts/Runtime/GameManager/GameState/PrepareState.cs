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
            _manager.CharacterFactory.ClearAll();
            int _boardWidth = 16;
            int _boardHeight = 16;
            _boardManager.SetBoardSize(_boardWidth, _boardHeight);
            _manager.PlayerSnake = new PlayerSnake(_manager);

            DataManager.Instance.SetTurn(0);

            _uiManager.HideAll();
            Debug.Log("OnEnter PrepareState");
            _boardManager.ClearData();
            _boardManager.GenerateObstacle(_manager.ObstacleRatio);

            List<SlotInfo> emptySlotList = _boardManager.BoardData.GetAllEmptySlots();
            emptySlotList.Shuffle();

            Character hero = _manager.RandomSpawnCharacter(emptySlotList, Team.PLAYER);
            //TODO: remove this line. it just for debug.
            hero.sprite.color = Color.green;
            hero.name = "head";
            _manager.PlayerSnake.AddCharacter(hero);

            _manager.Camera.FollowTarget(hero.transform);

            int initHeroAmount = DataManager.Instance.Config.InitSpawnHeroCount;
            _manager.RandomSpawnCharacterList(emptySlotList, Team.PLAYER, initHeroAmount);
            int initEnemyAmount = DataManager.Instance.Config.InitSpawnMonsterCount;
            _manager.RandomSpawnCharacterList(emptySlotList, Team.ENEMY, initEnemyAmount);

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
    }
}