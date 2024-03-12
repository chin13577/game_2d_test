using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class CharacterSpawner : MonoBehaviour
    {
        private GameManager _manager;

        private void OnEnable()
        {
            DataManager.OnUpdateTurn += DataManager_OnUpdateTurn;
        }

        private void OnDisable()
        {
            DataManager.OnUpdateTurn -= DataManager_OnUpdateTurn;
        }

        public void Init(GameManager manager)
        {
            _manager = manager;
        }

        private void DataManager_OnUpdateTurn(int turn)
        {
            GameConfig config = DataManager.Instance.Config;
            HandleSpawnEnemyLogic(turn, config);
            HandleSpawnHeroLogic(turn, config);
        }

        private void HandleSpawnEnemyLogic(int turn, GameConfig config)
        {
            bool isCanSpawnEnemy = turn > 0 && turn % config.SpawnEnemyTurn == 0;
            if (isCanSpawnEnemy)
            {
                //spawn enemy
                List<SlotInfo> emptySlotList = _manager.BoardManager.BoardData.GetAllEmptySlots();
                emptySlotList.Shuffle();

                int createAmount = DataManager.Instance.EnemySpawnWeight.Random();
                RandomSpawnCharacter(Team.ENEMY, emptySlotList, createAmount, config.SpawnMonsterLevelMultiplier);
            }
        }

        private void HandleSpawnHeroLogic(int turn, GameConfig config)
        {
            if (turn > 0 && turn % config.SpawnHeroTurn == 0)
            {
                List<SlotInfo> emptySlotList = _manager.BoardManager.BoardData.GetAllEmptySlots();
                emptySlotList.Shuffle();

                int createAmount = DataManager.Instance.EnemySpawnWeight.Random();
                RandomSpawnCharacter(Team.PLAYER, emptySlotList, createAmount, config.SpawnHeroLevelMultiplier);
            }
        }
        private void RandomSpawnCharacter(Team team, List<SlotInfo> emptySlotList, int createAmount, float levelMultiplier)
        {
            List<Character> characterList = _manager.RandomSpawnCharacterList(emptySlotList, team, createAmount);

            int heightestLevel = _manager.PlayerSnake.GetHighestLevel();
            for (int i = 0; i < characterList.Count; i++)
            {
                characterList[i].Status.Level = Mathf.Ceil(heightestLevel * levelMultiplier).ToInt32();
            }
        }

    }
}