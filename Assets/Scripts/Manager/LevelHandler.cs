using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelHandler : MonoBehaviour
{
    [SerializeField]
    private LevelData data;

    private bool isGameOver;

    public UnityEvent<bool> OnGameOver;

    [Header("In Game")]

    [SerializeField]
    private List<EnemyChar> enemies;

    [SerializeField]
    private PlayerChar player;
    public PlayerChar Player { get => player; }

    public List<BaseChar> AllChar
    {
        get
        {
            List<BaseChar> all = new List<BaseChar>();

            all.Add(player);
            all.AddRange(enemies);

            return all;
        }
    }

    private bool allEnemiesDie
    {
        get
        {
            foreach (BaseChar item in enemies)
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
        if (!player.IsDie) { return; }

        Debug.Log("LOSE");
        gameOver(false);
    }

    public void WinCheck()
    {
        if (!allEnemiesDie) { return; }

        Debug.Log("WIN");
        gameOver(true);
    }

    private void gameOver(bool isWin)
    {
        isGameOver = true;
        OnGameOver?.Invoke(isWin);
    }

    #endregion Win Lose

    public IEnumerator spawnEnemy()
    {
        for (int i = 0; i < data.enemies.Length; i++)
        {
            Vector3 pos = pointEnemy.position;
            pos.z -= i;
            pos.x -= i;
            EnemyChar _enemy = Instantiate(data.enemies[i], pos, Quaternion.identity, spawnParent).GetComponent<EnemyChar>();
            enemies.Add(_enemy);
            _enemy.Initialized(); // set data
        }

        yield return null;
    }

    public IEnumerator spawnPlayer()
    {
        PlayerChar _player = Instantiate(playerPrefab, pointPlayer.position, Quaternion.identity, spawnParent).GetComponent<PlayerChar>();
        player = _player;
        player.Initialized(); // set data

        isGameOver = false;

        yield return null;
    }

    public void ClearSpawn()
    {
        foreach (BaseChar baseChar in enemies)
        {
            Destroy(baseChar.gameObject);
        }
        enemies.Clear();

        Destroy(player.gameObject);
        player = null;
    }
}