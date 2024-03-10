using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{

    public class Hero : Character, IInteractable
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

        public void Interact(GameObject user)
        {
        }

        public void PostInteract(GameObject user)
        {
            // add to. player team.
            Hero hero = user.GetComponent<Hero>();
            if (hero != null)
            {
                GameManager.Instance.PlayerSnake.AddCharacter(this);
            }
        }

    }

}