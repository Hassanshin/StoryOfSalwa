using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField]
    private SaveBinary sav;
    private const string FILE_NAME = "sav";

    [ContextMenu("Save")]
    public void Save()
    {
        sav.Save(FILE_NAME, DataManager.Instance.playerData);
    }

    [ContextMenu("Load")]

    public PlayerData Load()
    {
        PlayerData loaded = (PlayerData)sav.Load(FILE_NAME);
        if (loaded == null)
        {
            Debug.Log("Load data null");
            return null;
        }
        return loaded;
    }
}