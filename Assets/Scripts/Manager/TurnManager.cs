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

    private int totalChars;
    private bool nextState = true;

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
        StartCoroutine(StartTurnEnum());
    }

    private IEnumerator StartTurnEnum()
    {
        for (int i = 0; i < totalChars; i++)
        {
            nextState = false;

            Debug.Log($"Turn {turnCount} = {charTurn[i]} : {charTurn[i].Speed}");

            yield return new WaitUntil(() => nextState);
        }
        yield return null;
        endTurn();
    }

    private void endTurn()
    {
        turnCount++;
        StartCoroutine(StartTurnEnum());
    }

    [ContextMenu("Next")]
    public void NextTurn()
    {
        nextState = true;
        //Debug.Log($"Next turn ");
    }

    // register to game manager
    public void ResetTurn()
    {
        charTurn.Clear();
        turnCount = 0;
    }
}