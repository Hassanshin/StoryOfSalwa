using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject GameArena;

    public LevelHandler Level;

    private void Start()
    {
        Level = GetComponent<LevelHandler>();
    }

    public void StarGame()
    {
        StartCoroutine(startGameCor());
    }

    public void BackToMain()
    {
        Level.ClearSpawn();
    }

    private IEnumerator startGameCor()
    {
        yield return Level.spawnPlayer();
        yield return Level.spawnEnemy();

        yield return TurnManager.Instance.RegisterTurn(Level.AllChar);
        // random deck?

        Debug.Log("Spawning done");
        GameArena.gameObject.SetActive(true);

        TurnManager.Instance.StartTurn();

        yield return null;
    }
}