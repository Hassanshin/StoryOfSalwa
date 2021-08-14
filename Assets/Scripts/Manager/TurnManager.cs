using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField]
    private int turnCount = 0;

    public int TurnCount => turnCount;

    [SerializeField]
    private List<BaseChar> charas = new List<BaseChar>();

    [SerializeField]
    private List<BaseChar> curCharas = new List<BaseChar>();

    private BaseChar curReadyChar;

    private int totalChars = 0;
    private bool nextState = true;

    private Coroutine turnCoroutine = null;
    private Coroutine postTurnCoroutine = null;

    public TurnManagerUI ui;

    public override void Initialization()
    {
        GameManager.Instance.Level.OnGameOver.AddListener(ResetTurn);
        ui = GetComponent<TurnManagerUI>();
    }

    public IEnumerator RegisterTurn(List<BaseChar> _charas)
    {
        charas.AddRange(_charas);
        charas.Sort((a, b) => b.s_Speed.CurValue.CompareTo(a.s_Speed.CurValue));

        curCharas.AddRange(charas);

        totalChars = charas.Count;

        // instantiate the UI from total Chars
        yield return ui.generateCharTurn(charas);

        // TODO: set them lerping from its speed along the max slider length
    }

    public void StartTurn()
    {
        turnCoroutine = StartCoroutine(StartTurnEnum());
    }

    private IEnumerator StartTurnEnum()
    {
        #region old

        /*
        for (int i = 0; i < totalChars; i++)
        {
            nextState = false;

            if (!curCharas[i].IsDie)
            {
                yield return attackingPhase(curCharas[i]);
            }
            else
            {
                nextState = true;
            }

            yield return new WaitUntil(() => nextState);
        }
        */

        #endregion old

        while (curCharas.Count > 0)
        {
            nextState = false;

            // looping all to find the first attaker
            yield return findAttacker();

            yield return new WaitUntil(() => nextState);
        }

        endTurn();
    }

    private IEnumerator findAttacker()
    {
        for (int i = 0; i < totalChars; i++)
        {
            if (curCharas[i].IsDie)
            {
                //nextState = true;
            }
            else
            {
                yield return attackingPhase(curCharas[i]);

                break;
            }
        }
    }

    private IEnumerator attackingPhase(BaseChar baseChar)
    {
        yield return new WaitForSeconds(0.1f);

        yield return ui.MoveToReady(baseChar);
        yield return baseChar.PreTurnBuff();

        curCharas.Remove(baseChar);

        // check die from buff
        if (baseChar.IsDie)
        {
            yield break;
        }

        curReadyChar = baseChar;

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
    }

    public IEnumerator SpeedEffectBuff(BaseChar baseChar, float amount)
    {
        ui.MovePosEffect(baseChar, amount);

        curCharas.Sort((a, b) => b.s_Speed.CurValue.CompareTo(a.s_Speed.CurValue));

        ui.RearrangeUi();

        yield return null;
    }

    private void endTurn()
    {
        turnCount++;

        restartTurn();
    }

    private void restartTurn()
    {
        charas.Sort((a, b) => b.s_Speed.CurValue.CompareTo(a.s_Speed.CurValue));
        curCharas.AddRange(charas);

        ui.RearrangeUi();

        turnCoroutine = StartCoroutine(StartTurnEnum());
    }

    [ContextMenu("Next")]
    public void NextTurn()
    {
        if (GameManager.Instance.Level.IsGameOver) { return; }
        postTurnCoroutine = StartCoroutine(curReadyChar.PostTurnBuff());
        curReadyChar = null;
        nextState = true;

        ui.MoveToTop();
        //SortSpeed();
        // TODO: CHECK SPEED
    }

    public void ResetTurn(bool isWin)
    {
        if (turnCoroutine != null)
            StopCoroutine(turnCoroutine);

        charas.Clear();
        curCharas.Clear();

        turnCount = 0;

        ui.resetUiTurn();
    }

    public void RemoveTurn(BaseChar removed)
    {
        curCharas.Remove(removed);
    }
}