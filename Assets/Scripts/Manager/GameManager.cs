using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent InitializationSequence;
    public UnityEvent OnBackToMenu;

    [Header("Components")]

    [SerializeField]
    private GameObject GameArena;

    public LevelHandler Level;
    public DeckManager Deck;

    private void Start()
    {
        Level = GetComponent<LevelHandler>();
        Deck = GetComponent<DeckManager>();

        InitializationSequence?.Invoke();

        OnBackToMenu.AddListener(Level.ClearSpawn);
        OnBackToMenu.AddListener(Deck.ClearDeck);
    }

    #region sequence manager

    // next

    #endregion sequence manager

    public void StarGame()
    {
        AudioManager.Instance.PlaySfx(0);

        StartCoroutine(startGameCor());
    }

    public void BackToMain()
    {
        AudioManager.Instance.PlaySfx(0);

        OnBackToMenu?.Invoke();
    }

    private IEnumerator startGameCor()
    {
        // TODO start loading

        yield return Deck.loadCardList();
        Debug.Log("Deck done");

        yield return Level.spawnPlayer();
        Debug.Log("Player done");

        yield return Level.spawnEnemy();
        Debug.Log("Enemy done");

        yield return TurnManager.Instance.RegisterTurn(Level.AllChar);
        Debug.Log("Turn done");

        // TODO end loading

        GameArena.gameObject.SetActive(true);

        TurnManager.Instance.StartTurn();

        yield return null;
    }
}