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
            DataManager.OnUpdateTurn += SpawnCharacterByGameTurn;
            Hero.OnHeroJoinTeam += Hero_OnHeroJoinTeam;
            Monster.OnMonsterDead += Monster_OnMonsterDead;
        }

        private void OnDisable()
        {
            DataManager.OnUpdateTurn -= SpawnCharacterByGameTurn;
            Hero.OnHeroJoinTeam -= Hero_OnHeroJoinTeam;
            Monster.OnMonsterDead -= Monster_OnMonsterDead;
        }

        public void Init(GameManager manager)
        {
            _manager = manager;
        }

        private void Hero_OnHeroJoinTeam()
        {
            GameConfig config = DataManager.Instance.Config;
            int createAmount = DataManager.Instance.SpawnOnHeroJoinWeight.Random();
            Debug.Log("spawn hero: " + createAmount);
            RandomSpawnCharacter(Team.PLAYER, createAmount, config.SpawnHeroLevelMultiplier);
        }

        private void Monster_OnMonsterDead()
        {
            GameConfig config = DataManager.Instance.Config;
            int createAmount = DataManager.Instance.SpawnOnEnemyDeadWeight.Random();
            Debug.Log("spawn monster: " + createAmount);
            RandomSpawnCharacter(Team.ENEMY, createAmount, config.SpawnMonsterLevelMultiplier);
        }

        #region Spawn By Turn Updated

        private void SpawnCharacterByGameTurn(int turn)
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
                int createAmount = DataManager.Instance.EnemySpawnByTurnWeight.Random();
                Debug.Log("spawn enemy: " + createAmount);
                RandomSpawnCharacter(Team.ENEMY, createAmount, config.SpawnMonsterLevelMultiplier);
            }
        }

        private void HandleSpawnHeroLogic(int turn, GameConfig config)
        {
            if (turn > 0 && turn % config.SpawnHeroTurn == 0)
            {
                int createAmount = DataManager.Instance.EnemySpawnByTurnWeight.Random();
                Debug.Log("spawn hero: " + createAmount);
                RandomSpawnCharacter(Team.PLAYER, createAmount, config.SpawnHeroLevelMultiplier);
            }
        }
        #endregion

        private void RandomSpawnCharacter(Team team, int createAmount, float levelMultiplier)
        {
            if (createAmount == 0)
                return;

            List<SlotInfo> emptySlotList = _manager.BoardManager.BoardData.GetAllEmptySlots();
            emptySlotList.Shuffle();
            List<Character> characterList = _manager.RandomSpawnCharacterList(emptySlotList, team, createAmount);

            int heightestLevel = _manager.PlayerSnake.GetHighestLevel();
            for (int i = 0; i < characterList.Count; i++)
            {
                characterList[i].Status.Level = Mathf.Ceil(heightestLevel * levelMultiplier).ToInt32();
            }
        }

    }
}