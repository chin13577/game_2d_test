using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FS
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private Coroutine _fadeTextCoroutine;
        private Coroutine _moveCoroutine;

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void PlayAtPosition(Vector3 pos)
        {
            this.transform.position = pos;
            StopAllTask();

            _moveCoroutine = StartCoroutine(MoveUP(0.8f, 1f));
            _fadeTextCoroutine = StartCoroutine(FadeText(0.8f, () =>
            {
                gameObject.SetActive(false);
            }));
        }

        private void StopAllTask()
        {
            if (_fadeTextCoroutine != null)
                StopCoroutine(_fadeTextCoroutine);
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
        }

        private IEnumerator MoveUP(float duration, float height, Action onComplete = null)
        {
            Vector3 startPos = this.transform.position;
            Vector3 endPos = startPos + new Vector3(0, height);

            float timer = 0f;
            while (duration > 0)
            {
                transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator FadeText(float duration, Action onComplete = null)
        {
            Color textColor = _text.color;
            textColor.a = 1;
            _text.color = textColor;

            float timer = 0f;
            while (duration > 0)
            {
                textColor.a = Mathf.Lerp(1, 0, timer / duration);
                timer += Time.deltaTime;
                _text.color = textColor;
                yield return null;
            }
        }
    }
}