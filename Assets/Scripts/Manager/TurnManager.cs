using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField]
    private float baseSliderTime = 100f;

    [SerializeField]
    private int turnCount = 0;

    [SerializeField]
    private List<BaseChar> charTurn = new List<BaseChar>();

    private int totalChars = 0;
    private bool nextState = true;

    private Coroutine turnCoroutine = null;

    public override void Initialization()
    {
        GameManager.Instance.Level.OnGameOver.AddListener(ResetTurn);
    }

    public IEnumerator RegisterTurn(List<BaseChar> chars)
    {
        chars.Sort((a, b) => b.Speed.CompareTo(a.Speed));
        charTurn.AddRange(chars);
        totalChars = charTurn.Count;

        // instantiate the UI from total Chars
        // set them lerping from its speed along the max slider length

        yield return null;
    }

    public void StartTurn()
    {
        turnCoroutine = StartCoroutine(StartTurnEnum());
    }

    private IEnumerator StartTurnEnum()
    {
        for (int i = 0; i < totalChars; i++)
        {
            nextState = false;

            if (!charTurn[i].IsDie)
            {
                Debug.Log($"Turn {turnCount} = {charTurn[i]} : {charTurn[i].Speed}");

                yield return attackingPhase(charTurn[i]);
            }
            else
            {
                Debug.Log($"Skipped Turn {turnCount} = {charTurn[i]} : {charTurn[i].Speed}");
                nextState = true;
            }

            yield return new WaitUntil(() => nextState);
        }
        yield return null;
        endTurn();
    }

    private IEnumerator attackingPhase(BaseChar baseChar)
    {
        yield return new WaitForSeconds(1);

        if (baseChar is EnemyChar)
        {
            EnemyChar enemy = baseChar as EnemyChar;

            enemy.Attack();
        }
        else if (baseChar is PlayerChar)
        {
            PlayerChar player = baseChar as PlayerChar;

            player.AttackPhase();
        }
        yield return null;
    }

    private void endTurn()
    {
        turnCount++;
        turnCoroutine = StartCoroutine(StartTurnEnum());
    }

    [ContextMenu("Next")]
    public void NextTurn()
    {
        nextState = true;
    }

    // register to game manager
    public void ResetTurn(bool isWin)
    {
        StopCoroutine(turnCoroutine);

        charTurn.Clear();
        turnCount = 0;
    }

    public void RemoveTurn(BaseChar removed)
    {
        charTurn.Remove(removed);
    }
}