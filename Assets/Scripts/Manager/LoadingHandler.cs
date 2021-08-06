using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class LoadingHandler : Singleton<LoadingHandler>
{
    [SerializeField]
    private Canvas loadingCanvas;

    [SerializeField]
    private TextMeshProUGUI loadingText;

    [SerializeField]
    private Image loadingBar;

    [SerializeField]
    private UnityEvent finishedEvent;

    public void ShowLoading(UnityAction doneAction = null)
    {
        loadingCanvas.gameObject.SetActive(true);
        if (doneAction != null)
        {
            finishedEvent.AddListener(doneAction);
            //doneLoading += doneAction();
        }
    }

    public void Progress(float prog)
    {
        loadingBar.fillAmount = prog;
        loadingText.text = $"{prog * 100}";
    }

    public void FinishedLoading()
    {
        loadingCanvas.gameObject.SetActive(false);
        finishedEvent?.Invoke();
    }

    public void FailLoading(string err)
    {
        loadingText.text = $"{err}";
    }
}