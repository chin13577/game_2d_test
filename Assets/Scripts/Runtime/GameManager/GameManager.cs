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
        // Start is called before the first frame update
        void Start()
        {
            _boardManager.Init();
            _boardManager.SetBoardSize(_boardWidth, _boardHeight);
            _boardManager.GenerateObstacle(6);
        }

    }
}
