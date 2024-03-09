using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{

    public class Obstacle : MonoBehaviour
    {
        private ObstacleData _data;

        [Header("Debug")]
        [SerializeField] private int col;
        [SerializeField] private int row;

        public void SetData(ObstacleData data)
        {
            this._data = data;

            this.col = data.Col;
            this.row = data.Row;
        }
    }

}