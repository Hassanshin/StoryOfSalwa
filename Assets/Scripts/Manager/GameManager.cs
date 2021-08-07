using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject GameArena;

    public LevelHandler level;

    public void StarGame()
    {
        StartCoroutine(startGameCor());
    }

    public void BackToMain()
    {
        level.ClearSpawn();
    }

    private IEnumerator startGameCor()
    {
        yield return level.spawnPlayer();
        yield return level.spawnEnemy();

        // random deck?

        Debug.Log("Spawning done");
        GameArena.gameObject.SetActive(true);

        yield return null;
    }
}