using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    public SpriteRenderer sprite;
    public override Team Team => Team.PLAYER;

    public void SetData()
    {
        //TODO: implement set some status and data.
    }

    public void SetSprite(string spriteId)
    {
        //TODO: load sprite async.
    }
}
