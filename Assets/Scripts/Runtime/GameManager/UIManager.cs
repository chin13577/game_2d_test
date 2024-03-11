using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public StartScreenUI StartScreenUI { get => _startScreenUI; }
    [SerializeField] private StartScreenUI _startScreenUI;
}
