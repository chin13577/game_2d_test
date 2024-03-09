using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PixelCamera2DFollower _camera;
        [SerializeField] private BoardManager _boardManager;

        [Header("Config")]
        [SerializeField] private int _boardWidth = 16;
        [SerializeField] private int _boardHeight = 16;
        [Range(0, 1f)] public float ObstacleRatio = 0.1f;

        public Character characterPrefab;

        public Hero playerTest;
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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _boardManager.ClearData();
                _boardManager.GenerateObstacle(ObstacleRatio);

                List<BoardObject> emptySlotList = _boardManager.BoardData.GetAllEmptySlots();
                playerTest = Instantiate(characterPrefab).GetComponent<Hero>();

                emptySlotList.Shuffle();

                Debug.Log("col: " + emptySlotList[0].Col + " row: " + emptySlotList[0].Row);
                playerTest.CurrentPosition = emptySlotList[0].WorldPos;
                Debug.Log("world pos: " + emptySlotList[0].WorldPos.x + " " + emptySlotList[0].WorldPos.y);
                emptySlotList[0].SetObject(playerTest.gameObject);

                _camera.FollowTarget(playerTest.transform);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                BoardObject boardObj = _boardManager.BoardData.GetBoardObjectFromPosition(playerTest.CurrentPosition);
                Debug.Log("world pos: " + boardObj.WorldPos.x + " " + boardObj.WorldPos.y);
                boardObj.Clear();
                playerTest.Move(Direction.Left);
                Debug.Log("playerTest world pos: " + playerTest.CurrentPosition.x + " " + playerTest.CurrentPosition.y);
                BoardObject newBoard = _boardManager.BoardData.GetBoardObjectFromPosition(playerTest.CurrentPosition);
                Debug.Log("world pos: " + newBoard.WorldPos.x + " " + newBoard.WorldPos.y);
                //newBoard.SetObject(playerTest.gameObject);
            }
        }
    }
}
