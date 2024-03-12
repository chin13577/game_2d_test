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
            InitPlayer(emptySlotList);

            SpawnInitialCharacters(emptySlotList);
            SetUIEvent();
        }

        private void InitPlayer(List<SlotInfo> emptySlotList)
        {
            Character hero = _manager.CharacterSpawner.RandomSpawnCharacter(emptySlotList, Team.PLAYER);
            _manager.PlayerSnake.AddCharacter(hero);
            _manager.Camera.FollowTarget(hero.transform);
        }

        private void SpawnInitialCharacters(List<SlotInfo> emptySlotList)
        {
            int initHeroAmount = DataManager.Instance.Config.InitSpawnHeroCount;
            _manager.CharacterSpawner.RandomSpawnCharacterList(emptySlotList, Team.PLAYER, initHeroAmount);
            int initEnemyAmount = DataManager.Instance.Config.InitSpawnMonsterCount;
            _manager.CharacterSpawner.RandomSpawnCharacterList(emptySlotList, Team.ENEMY, initEnemyAmount);
        }

        private void SetUIEvent()
        {
            StartScreenUI startScreenUI = this._uiManager.StartScreenUI;
            TutorialUI tutorialUI = this._uiManager.TutorialUI;
            startScreenUI.Show();
            startScreenUI.startBtn.SetCallback(() =>
            {
                startScreenUI.Hide();
                _manager.ChangeState(GameState.NORMAL);
            });
            startScreenUI.tutorialBtn.SetCallback(() =>
            {
                startScreenUI.Hide();
                tutorialUI.Show(() =>
                {
                    tutorialUI.Hide();
                    startScreenUI.Show();
                });
            });
        }

        public override void OnExit()
        {
            Debug.Log("OnExit PrepareState");
        }
    }
}