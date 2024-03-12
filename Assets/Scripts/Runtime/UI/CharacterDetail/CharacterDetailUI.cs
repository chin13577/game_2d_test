using FS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterDetailUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private TextMeshProUGUI _levelText;
    public void SetCharacter(Character character)
    {
        this._image.sprite = character.sprite.sprite;
        UpdateStatusUI(character.Status);
    }

    public void UpdateStatusUI(Status status)
    {
        _levelText.text = status.Level.ToString();
        _statusText.text = GetStatusText(status);
    }

    private string GetStatusText(Status status)
    {
        string text = "";
        text += $"HP: {status.HP}/{status.TotalMaxHP}\n";
        text += $"EXP: {status.EXP}/{status.MaxEXP}\n";
        text += $"\nAtk: {status.TotalAtk}";
        return text;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
