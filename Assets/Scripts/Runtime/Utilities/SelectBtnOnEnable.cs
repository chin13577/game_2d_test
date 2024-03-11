using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectBtnOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(this.gameObject);
    }
}
