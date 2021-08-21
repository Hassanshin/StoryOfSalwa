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
    public bool IsGameOver => isGameOver;

    public UnityEvent<bool> OnGameOver;

    [Header("In Game")]

    //[SerializeField]
    private List<EnemyChar> enemies = new List<EnemyChar>();

    //[SerializeField]
    private PlayerChar player;
    public PlayerChar Player { get => player; }

    public List<BaseChar> AllChar
    {
        get
        {
            List<BaseChar> all = new List<BaseChar>
            {
                player
            };
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

    [Header("Prefabs")]
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [Header("Components")]
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
            EnemyChar _enemy = Instantiate(enemyPrefab, enemyPosGenerated(i), Quaternion.identity, spawnParent).GetComponent<EnemyChar>();

            enemies.Add(_enemy);
            _enemy.SetData(data.enemies[i]); // set data
        }

        yield return null;
    }

    private Vector3 enemyPosGenerated(int i)
    {
        Vector3 pos = pointEnemy.position;
        pos.z -= i * 2;
        pos.x -= (i % 2 == 0 ? -1 : 1) * 2;
        return pos;
    }

    public IEnumerator spawnPlayer()
    {
        PlayerChar _player = Instantiate(playerPrefab, pointPlayer.position, Quaternion.identity, spawnParent).GetComponent<PlayerChar>();
        player = _player;

        player.SetData(data.player);

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

    public List<BaseChar> GetEnemiesInRange(BaseChar target, int range)
    {
        List<BaseChar> x = new List<BaseChar>();

        if (range <= 0)
        {
            x.Add(target);
        }
        else if (range >= 2)
        {
            x.AddRange(enemies);
        }
        else
        {
            int targetIndex = enemies.IndexOf((EnemyChar)target);

            for (int i = targetIndex - 1; i < targetIndex + 2; i++)
            {
                if (i < 0 || i >= enemies.Count) { continue; }
                x.Add(enemies[i]);
            }
        }

        return x;
    }
}