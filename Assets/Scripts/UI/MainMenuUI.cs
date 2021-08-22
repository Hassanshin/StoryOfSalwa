using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : Singleton<MainMenuUI>
{
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="index">0 Main, 1 Game, 2 Video, 3 Shop</param>
    public void DisplayOnly(int index)
    {
        for (int i = 0; i < panel.Count; i++)
        {
            panel[i].gameObject.SetActive(i == index);
        }
    }
}