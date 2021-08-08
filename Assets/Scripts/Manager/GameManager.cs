using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent InitializationSequence;
    public UnityEvent OnBackToMenu;

    [Header("Components")]

    [SerializeField]
    private GameObject GameArena;

    public LevelHandler Level;

    private void Start()
    {
        Level = GetComponent<LevelHandler>();
        InitializationSequence?.Invoke();

        OnBackToMenu.AddListener(Level.ClearSpawn);
    }

    #region sequence manager

    // next

    #endregion sequence manager

    public void StarGame()
    {
        StartCoroutine(startGameCor());
    }

    public void BackToMain()
    {
        OnBackToMenu?.Invoke();
    }

    private IEnumerator startGameCor()
    {
        // TODO start loading
        yield return Level.spawnPlayer();

        yield return Level.spawnEnemy();

        yield return TurnManager.Instance.RegisterTurn(Level.AllChar);
        // shuffle deck

        Debug.Log("Spawning done");
        // TODO end loading

        GameArena.gameObject.SetActive(true);

        TurnManager.Instance.StartTurn();

        yield return null;
    }
}