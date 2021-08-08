using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField]
    private int turnCount = 0;

    [SerializeField]
    private List<BaseChar> characters = new List<BaseChar>();
    public List<BaseChar> Characters { get => characters; }

    private int totalChars = 0;
    private bool nextState = true;

    private Coroutine turnCoroutine = null;

    public TurnManagerUI ui;

    public override void Initialization()
    {
        GameManager.Instance.Level.OnGameOver.AddListener(ResetTurn);
        ui = GetComponent<TurnManagerUI>();
    }

    public IEnumerator RegisterTurn(List<BaseChar> chars)
    {
        chars.Sort((a, b) => b.Speed.CompareTo(a.Speed));
        characters.AddRange(chars);
        totalChars = characters.Count;

        // instantiate the UI from total Chars
        yield return ui.generateCharTurn(totalChars);

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

            if (!characters[i].IsDie)
            {
                //Debug.Log($"Turn {turnCount} = {characters[i]} : {characters[i].Speed}");

                yield return attackingPhase(characters[i]);
            }
            else
            {
                //Debug.Log($"Skipped Turn {turnCount} = {characters[i]} : {characters[i].Speed}");

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

        yield return ui.MoveToReady(baseChar);

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

    public void ResetTurn(bool isWin)
    {
        StopCoroutine(turnCoroutine);

        characters.Clear();
        turnCount = 0;

        ui.resetUiTurn();
    }

    public void RemoveTurn(BaseChar removed)
    {
        characters.Remove(removed);
    }
}