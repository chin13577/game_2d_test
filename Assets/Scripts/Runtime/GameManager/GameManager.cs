using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class GameManager : MonoBehaviour
    {
        public AudioSource bgmAudioSource;
        public PixelCamera2DFollower Camera { get => _camera; }
        [SerializeField] private PixelCamera2DFollower _camera;
        public BoardManager BoardManager { get => _boardManager; }
        [SerializeField] private BoardManager _boardManager;
        public CharacterFactory CharacterFactory { get => _characterFactory; }
        [SerializeField] private CharacterFactory _characterFactory;
        public DamageTextFactory DamageTextFactory { get => _damageTextFactory; }
        [SerializeField] private DamageTextFactory _damageTextFactory;

        public CharacterSpawner CharacterSpawner { get => _characterSpawner; }
        [SerializeField] private CharacterSpawner _characterSpawner;
        public UIManager UIManager { get => _uiManager; }
        [SerializeField] private UIManager _uiManager;

        public PlayerSnake PlayerSnake { get; set; }

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
            LoadBGMAndPlay();

            BoardManager.Init();
            CharacterFactory.Init();
            DamageTextFactory.Init();
            CharacterSpawner.Init(this);
            ChangeState(GameState.PREPARE);
        }

        private void LoadBGMAndPlay()
        {
            ResourceManager.Instance.GetAsset<AudioClip>("BGM/octopath-bgm.mp3", (AudioClip clip) =>
            {
                if (clip != null)
                {
                    this.bgmAudioSource.clip = clip;
                    this.bgmAudioSource.Play();
                }
            });
        }

        void OnDisable()
        {
            _currentState?.OnExit();
        }

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
            DataManager.Instance.UpdateTurn();
        }

    }
}
