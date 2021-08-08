using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    [SerializeField]
    private List<CardData> inGameCardList = new List<CardData>();

    [SerializeField]
    private Image blocker;

    public void StateActive(bool _state = true)
    {
        blocker.gameObject.SetActive(!_state);
    }
}