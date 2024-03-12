using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class GameManager : MonoBehaviour
    {
        public static event Action<int> OnUpdateTurn;

        public PixelCamera2DFollower Camera { get => _camera; }
        [SerializeField] private PixelCamera2DFollower _camera;
        public BoardManager BoardManager { get => _boardManager; }
        [SerializeField] private BoardManager _boardManager;
        public CharacterFactory CharacterFactory { get => _characterFactory; }
        [SerializeField] private CharacterFactory _characterFactory;

        public UIManager UIManager { get => _uiManager; }
        [SerializeField] private UIManager _uiManager;

        [Header("Config")]
        [SerializeField] private int _boardWidth = 16;
        [SerializeField] private int _boardHeight = 16;
        [Range(0, 1f)] public float ObstacleRatio = 0.1f;

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
            _characterFactory.Init();
            _boardManager.Init();
            _boardManager.SetBoardSize(_boardWidth, _boardHeight);
            PlayerSnake = new PlayerSnake(this);

            ChangeState(GameState.PREPARE);
        }

        void OnDisable()
        {
            _currentState?.OnExit();
        }

        public Direction playerInputDir = Direction.Up;

        private GameStateBase _currentState;
        private int _turn = 0;

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
                    return new PrepareState(this);
                case GameState.NORMAL:
                    return new NormalState(this);
                case GameState.BATTLE:
                    return new BattleState(this);
                case GameState.RESULT:
                    return new ResultState(this);
                default:
                    return null;
            }
        }

      
        public void OnPlayerMove()
        {
            UpdateTurn();
        }

        public void UpdateTurn()
        {
            this._turn += 1;
            OnUpdateTurn?.Invoke(this._turn);
        }
        public void SetTurn(int turn)
        {
            this._turn = turn;
        }

        public int GetTurn()
        {
            return _turn;
        }
    }
}
