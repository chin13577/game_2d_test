using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class CharacterFactory : MonoBehaviour
    {
        [SerializeField] private Hero _heroPrefab;
        [SerializeField] private Monster _enemyPrefab;

        private FlexiblePooling<Character> _monsterPooling;
        private FlexiblePooling<Character> _heroPooling;

        private bool _isInit = false;
        public void Init()
        {
            if (_isInit == false)
            {
                _isInit = true;
                this._heroPooling = new FlexiblePooling<Character>(null, _heroPrefab, 1);
                this._monsterPooling = new FlexiblePooling<Character>(null, _enemyPrefab, 1);
            }
        }


        public Character GetHero()
        {
            return this._heroPooling.GetObject();
        }

        public Character GetMonster()
        {
            return this._monsterPooling.GetObject();
        }

        public void ClearAll()
        {
            this._heroPooling.HideAllObject();
            this._monsterPooling.HideAllObject();
        }

    }
}