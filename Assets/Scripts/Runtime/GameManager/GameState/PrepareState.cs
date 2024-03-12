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

            GameConfig config = DataManager.Instance.Config;
            _boardManager.SetBoardSize(config.BoardWidth, config.BoardHeight);
            _manager.PlayerSnake = new PlayerSnake(_manager);

            DataManager.Instance.SetTurn(0);
            DataManager.Instance.ResetKillCount();

            _uiManager.HideAll();
            Debug.Log("OnEnter PrepareState");
            _boardManager.ClearData();

            float obstacleRatio = DataManager.Instance.Config.ObstacleRatio;
            _boardManager.GenerateObstacle(obstacleRatio);

            List<SlotInfo> emptySlotList = _boardManager.BoardData.GetAllEmptySlots();
            emptySlotList.Shuffle();

            Character hero = _manager.CharacterSpawner.RandomSpawnCharacter(emptySlotList, Team.PLAYER);
            _manager.PlayerSnake.AddCharacter(hero);
            _manager.Camera.FollowTarget(hero.transform);

            int initHeroAmount = DataManager.Instance.Config.InitSpawnHeroCount;
            _manager.CharacterSpawner.RandomSpawnCharacterList(emptySlotList, Team.PLAYER, initHeroAmount);
            int initEnemyAmount = DataManager.Instance.Config.InitSpawnMonsterCount;
            _manager.CharacterSpawner.RandomSpawnCharacterList(emptySlotList, Team.ENEMY, initEnemyAmount);

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