using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    [SerializeField]
    private Image blocker;

    [SerializeField]
    private TextMeshProUGUI topText;

    [SerializeField]
    private Button endButton;

    [Header("Components")]
    [SerializeField]
    private List<CardUI> cardUiList = new List<CardUI>();

    [SerializeField]
    private RectTransform graveStack;

    [SerializeField]
    private RectTransform deckStack;

    //[SerializeField]
    //private GameObject cardUiPrefab;

    //[SerializeField]
    //private Transform cardUiParent;

    public void Initialize()
    {
        endButton.onClick.AddListener(endTurnButton);
    }

    public IEnumerator applyHandCard(int index, CardData card)
    {
        // draw animation
        yield return new WaitForSeconds(0.1f);

        cardUiList[index].SetCardData(card);

        yield return null;
    }

    public IEnumerator applyUsedCard(int index)
    {
        // deleted animation
        yield return new WaitForSeconds(0.1f);

        cardUiList[index].SetBlank();

        yield return null;
    }

    public void ClearDeck()
    {
        for (int i = 0; i < cardUiList.Count; i++)
        {
            cardUiList[i].SetBlank();
        }
    }

    public void StateActive(bool _state = true)
    {
        blocker.gameObject.SetActive(!_state);
        endButton.interactable = _state;
    }

    private void endTurnButton()
    {
        GameManager.Instance.Level.Player.EndTurnButton();
    }

    public void SetTopText(string _text)
    {
        topText.text = _text;
    }

    public void UpdateNumber(int deck, int grave)
    {
        deckStack.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{deck}";
        graveStack.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{grave}";
    }
}