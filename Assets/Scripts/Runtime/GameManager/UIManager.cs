using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public StartScreenUI StartScreenUI { get => _startScreenUI; }
    [SerializeField] private StartScreenUI _startScreenUI;
    public ResultUI ResultUI { get => _resultUI; }
    [SerializeField] private ResultUI _resultUI;

    public GameUI GameUI { get => _gameUI; }
    [SerializeField] private GameUI _gameUI;

    public void HideAll()
    {
        StartScreenUI.Hide();
        ResultUI.Hide();
        GameUI.Hide();
    }
}
