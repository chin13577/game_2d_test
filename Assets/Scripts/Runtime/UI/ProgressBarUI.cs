using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image _imageFill;
    [SerializeField] private TextMeshProUGUI _text;

    public void SetProgress(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        _imageFill.fillAmount = ratio;
        _text.text = $"{ratio * 100f} %";
    }
}
