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
        [SerializeField] private int _boardWidth = 15;
        [SerializeField] private int _boardHeight = 15;
        // Start is called before the first frame update
        void Start()
        {
            _boardManager.SetBoardSize(_boardWidth, _boardHeight);
            _boardManager.GenerateObstacle();
        }

    }
}
