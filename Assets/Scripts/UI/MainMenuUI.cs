using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : Singleton<MainMenuUI>
{
    /// <summary>
    /// 0 Main, 1 Game, 2 Video
    /// </summary>
    public List<Canvas> panel = new List<Canvas>();

    public override void Initialization()
    {
    }

    public void setVideoPanel(bool state)
    {
        panel[2].gameObject.SetActive(state);
    }

    public void PlayGame()
    {
        GameManager.Instance.StarGame();
    }

    public void BackToMain()
    {
        panel[0].gameObject.SetActive(true);
        panel[1].gameObject.SetActive(false);
        GameManager.Instance.BackToMain();
    }
}