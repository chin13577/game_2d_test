using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public List<PageUI> pageUIList = new List<PageUI>();

    public void Show(Action onComplete)
    {
        this.gameObject.SetActive(true);
        SetPageEvent(onComplete);
        HideAllPage();
        pageUIList[0].Show();
    }

    public void SetPageEvent(Action onReadTutorialComplete)
    {
        for (int i = 0; i < pageUIList.Count; i++)
        {
            int index = i;
            SetEventPreviousPage(index);
            SetEventNextPage(onReadTutorialComplete, index);
        }
    }

    private void SetEventPreviousPage(int index)
    {
        if (pageUIList[index].previousButton == null)
            return;
        pageUIList[index].previousButton.SetCallback(() =>
        {
            HideAllPage();
            if (index > 0)
            {
                pageUIList[index - 1].Show();
            }
        });
    }

    private void SetEventNextPage(Action onReadTutorialComplete, int index)
    {
        if (pageUIList[index].nextButton == null)
            return;
        pageUIList[index].nextButton.SetCallback(() =>
        {
            HideAllPage();
            if (index + 1 < pageUIList.Count)
            {
                pageUIList[index + 1].Show();
            }
            bool lastPage = index == pageUIList.Count - 1;
            if (lastPage)
            {
                onReadTutorialComplete?.Invoke();
            }
        });
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void HideAllPage()
    {
        for (int i = 0; i < pageUIList.Count; i++)
        {
            pageUIList[i].Hide();
        }
    }
}
