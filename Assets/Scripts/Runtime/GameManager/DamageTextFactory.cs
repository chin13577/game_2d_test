using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public class DamageTextFactory : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;
        [SerializeField] private Transform _spawnParent;
        private FlexiblePooling<DamageText> _textPool;

        private bool _isInit = false;
        public void Init()
        {
            if (_isInit == false)
            {
                _isInit = true;
                this._textPool = new FlexiblePooling<DamageText>(_spawnParent, _damageTextPrefab, 1);
            }
        }

        public void SpawnText(string text, Vector3 pos)
        {
            DamageText damageText = this._textPool.GetObject();
            damageText.SetText(text);
            damageText.PlayAtPosition(pos);
        }

        public void ClearAll()
        {
            this._textPool.HideAllObject();
        }

    }
}