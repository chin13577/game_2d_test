using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public StartScreenUI StartScreenUI { get => _startScreenUI; }
    [SerializeField] private StartScreenUI _startScreenUI;
    public ResultUI ResultUI { get => _resultUI; }
    [SerializeField] private ResultUI _resultUI;
}
