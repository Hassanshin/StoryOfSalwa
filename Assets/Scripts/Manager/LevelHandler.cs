using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelHandler : MonoBehaviour
{
    [SerializeField]
    private LevelData data;

    private bool isGameOver;

    [Header("In Game")]

    public List<BaseChar> Enemies;

    public BaseChar player;

    private bool allEnemiesDie
    {
        get
        {
            foreach (BaseChar item in Enemies)
            {
                if (!item.IsDie) { return false; }
            }
            return true;
        }
    }

    [Header("Components")]
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform spawnParent;

    [SerializeField]
    private Transform pointPlayer;

    [SerializeField]
    private Transform pointEnemy;

    public void SetLevelData(LevelData _data)
    {
        data = _data;
    }

    #region Win Lose

    public void LoseCheck()
    {
        Debug.Log("LOSE");
    }

    public void WinCheck()
    {
        Debug.Log("WIN");
    }

    public void GameOverCheck()
    {
        if (isGameOver) { return; }

        if (player.IsDie)
        {
            LoseCheck();
            isGameOver = true;
        }
        else if (allEnemiesDie)
        {
            WinCheck();
            isGameOver = true;
        }
    }

    #endregion Win Lose

    public IEnumerator spawnEnemy()
    {
        for (int i = 0; i < data.enemies.Length; i++)
        {
            Vector3 pos = pointEnemy.position;
            pos.z -= i;
            pos.x -= i;
            BaseChar _enemy = Instantiate(data.enemies[i], pos, Quaternion.identity, spawnParent).GetComponent<BaseChar>();
            Enemies.Add(_enemy);
        }
        yield return null;
    }

    public IEnumerator spawnPlayer()
    {
        BaseChar _player = Instantiate(playerPrefab, pointPlayer.position, Quaternion.identity, spawnParent).GetComponent<BaseChar>();
        player = _player;

        isGameOver = false;

        yield return null;
    }

    internal void ClearSpawn()
    {
        foreach (BaseChar baseChar in Enemies)
        {
            Destroy(baseChar.gameObject);
        }
        Enemies.Clear();

        Destroy(player.gameObject);
        player = null;
    }
}