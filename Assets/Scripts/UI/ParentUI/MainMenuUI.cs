using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : Singleton<MainMenuUI>
{
    [SerializeField]
    private List<Canvas> panel = new List<Canvas>();

    [SerializeField]
    private GameObject game;

    [SerializeField]
    private RawImage video;

    public RawImage Video { get => video; }

    public void PlayGame()
    {
        panel[0].gameObject.SetActive(false);
        panel[1].gameObject.SetActive(true);
        game.SetActive(true);
    }

    public void BackToMain()
    {
        panel[0].gameObject.SetActive(true);
        panel[1].gameObject.SetActive(false);
        game.SetActive(false);
    }
}