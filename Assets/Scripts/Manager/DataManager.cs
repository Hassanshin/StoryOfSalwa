using System;
using System.Collections;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerData playerData;

    public bool IsNewPlayer
    {
        get
        {
            return String.IsNullOrEmpty(playerData.PlayerName);
        }
    }

    private void Start()
    {
        playerData = SaveManager.Instance.Load();
        if (IsNewPlayer)
        {
            Debug.Log("NEW PLAYER");
            playerData.PlayerName = "Naruto";
        }
        else
        {
            Debug.Log("LOADED");
        }

        TimeManager.Instance.Initialization();
    }
}