using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public CustomButton resetBtn;

    //TODO: set result.
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
