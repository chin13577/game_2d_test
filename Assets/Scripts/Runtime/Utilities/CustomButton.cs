using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] public Button button = null;
    [SerializeField] public TextMeshProUGUI text = null;

    [Header("Optional")]
    [SerializeField] public string sfxKey = "";


    public void SetCallback(Action onClick)
    {
        this.button.onClick.RemoveAllListeners();
        this.button.onClick.AddListener(() =>
        {
            onClick?.Invoke();
        });
    }

    public void SetText(string text)
    {
        if (this.text)
            this.text.text = text;
    }

    public void SetInteractive(bool interactable)
    {
        this.button.interactable = interactable;
    }
}
