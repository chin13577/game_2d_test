using FS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    public CharacterDetailUI PlayerDetailUI { get => _playerDetailUI; }
    [SerializeField] private CharacterDetailUI _playerDetailUI;
    public CharacterDetailUI EnemyDetailUI { get => _enemyDetailUI; }
    [SerializeField] private CharacterDetailUI _enemyDetailUI;

    public void SetTurnText(int turn)
    {
        _turnText.text = $"Turn\n{turn.ToString()}";
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
