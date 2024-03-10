using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{

    public class Monster : Character
    {
        public override Team Team => Team.ENEMY;
    }

}