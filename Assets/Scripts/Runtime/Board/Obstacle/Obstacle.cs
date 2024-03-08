using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{

    public class Obstacle : MonoBehaviour
    {
        private ObstacleData _data;

        public void SetData(ObstacleData data)
        {
            this._data = data;
        }
    }

}