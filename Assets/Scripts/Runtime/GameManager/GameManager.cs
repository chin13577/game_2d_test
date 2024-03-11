using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class GameManager : MonoBehaviour
    {
        public static event Action OnUpdateTurn;

        public PixelCamera2DFollower Camera { get => _camera; }
        [SerializeField] private PixelCamera2DFollower _camera;
        public BoardManager BoardManager { get => _boardManager; }
        [SerializeField] private BoardManager _boardManager;

        [Header("Config")]
        [SerializeField] private int _boardWidth = 16;
        [SerializeField] private int _boardHeight = 16;
        [Range(0, 1f)] public float ObstacleRatio = 0.1f;

        public Character characterPrefab;
        public PlayerSnake PlayerSnake { get; private set; }

        // Start is called before the first frame update

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<GameManager>();
                return _instance;
            }
        }


        public static GameManager _instance;

        void Start()
        {
            _boardManager.Init();
            _boardManager.SetBoardSize(_boardWidth, _boardHeight);
            PlayerSnake = new PlayerSnake(this);

            ChangeState(GameState.NORMAL);
        }

        void OnDisable()
        {
            _currentState?.OnExit();
        }

        public Direction playerInputDir = Direction.Up;

        private GameStateBase _currentState;
        public void ChangeState(GameState state)
        {
            _currentState?.OnExit();
            _currentState = GetGameState(state);
            _currentState.OnEnter();
        }

        private GameStateBase GetGameState(GameState state)
        {
            switch (state)
            {
                case GameState.PREPARE:

                case GameState.NORMAL:
                    return new NormalState(this);
                case GameState.BATTLE:
                    return new BattleState(this);
                case GameState.RESULT:
                    return new ResultState(this);
                default:
                    return null;
            }
            return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _boardManager.ClearData();
                _boardManager.GenerateObstacle(ObstacleRatio);

                List<SlotInfo> emptySlotList = _boardManager.BoardData.GetAllEmptySlots();
                emptySlotList.Shuffle();

                Hero hero = RandomSpawnHero(emptySlotList);
                hero.sprite.color = Color.red;
                hero.name = "head";
                PlayerSnake.AddCharacter(hero);

                _camera.FollowTarget(hero.transform);

                RandomSpawnHero(emptySlotList);
                RandomSpawnHero(emptySlotList);
                RandomSpawnHero(emptySlotList);
                RandomSpawnHero(emptySlotList);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                playerInputDir = Direction.Left;
                _currentState?.OnPlayerUpdateInputDirection(playerInputDir);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                playerInputDir = Direction.Right;
                _currentState?.OnPlayerUpdateInputDirection(playerInputDir);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                playerInputDir = Direction.Up;
                _currentState?.OnPlayerUpdateInputDirection(playerInputDir);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                playerInputDir = Direction.Down;
                _currentState?.OnPlayerUpdateInputDirection(playerInputDir);
            }
        }

        private Hero RandomSpawnHero(List<SlotInfo> emptySlotList)
        {
            SlotInfo slot = emptySlotList[0];
            emptySlotList.RemoveAt(0);

            Hero hero = Instantiate(characterPrefab).GetComponent<Hero>();
            hero.Init();
            hero.CurrentPosition = slot.WorldPos;
            slot.SetObject(hero);

            return hero;
        }
    }
}
