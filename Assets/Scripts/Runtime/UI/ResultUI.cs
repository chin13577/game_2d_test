using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public CustomButton resetBtn;

    [SerializeField] private TextMeshProUGUI _resultText;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetResultText(int enemyKillCount, int turn)
    {
        string result = $"Enemy Kill Count: {enemyKillCount}\n";
        result += $"Total Turn: {turn}";
        this._resultText.text = result;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
