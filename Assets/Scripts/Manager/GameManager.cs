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

    [SerializeField]
    private GameObject NamePanel;

    [SerializeField]
    private TMPro.TMP_InputField nameInput;

    [Header("Handlers")]

    public LevelHandler Level;
    public DeckManager Deck;

    private void Start()
    {
        Level = GetComponent<LevelHandler>();
        Deck = GetComponent<DeckManager>();

        InitializationSequence?.Invoke();

        OnBackToMenu.AddListener(Level.ClearSpawn);
        OnBackToMenu.AddListener(Deck.ClearDeck);

        nameInput.onEndEdit.AddListener(SetName);
        NamePanel.SetActive(SaveManager.Instance.IsNewPlayer);
    }

    #region sequence manager

    // next

    #endregion sequence manager

    public void StarGame()
    {
        AudioManager.Instance.PlaySfx(0);

        StartCoroutine(StartGameNumerator());
    }

    public void BackToMain()
    {
        AudioManager.Instance.PlaySfx(0);

        Level.OnGameOver?.Invoke(false);
        OnBackToMenu?.Invoke();
    }

    private IEnumerator StartGameNumerator()
    {
        yield return LoadingHandler.Instance.ShowLoadingEnum();

        yield return ObjectPool.Instance.GeneratingPool();

        yield return Deck.loadCardList();
        //Debug.Log("Deck done");

        yield return Level.spawnPlayer();
        //Debug.Log("Player done");

        yield return Level.spawnEnemy();
        //Debug.Log("Enemy done");

        yield return TurnManager.Instance.RegisterTurn(Level.AllChar);
        //Debug.Log("Turn done");

        MainMenuUI.Instance.panel[0].gameObject.SetActive(false);
        GameArena.gameObject.SetActive(true);

        yield return LoadingHandler.Instance.FinishedLoadingEnum();

        // TODO: countdown?
        yield return new WaitForSeconds(1f);

        MainMenuUI.Instance.panel[1].gameObject.SetActive(true);
        TurnManager.Instance.StartTurn();
    }

    public void SetName(string name)
    {
        SaveManager.Instance.playerData.PlayerName = name;
    }

    public void DoneEditingName()
    {
        SaveManager.Instance.playerData.PlayerName = nameInput.text;
        SaveManager.Instance.Save();

        NamePanel.SetActive(false);
    }
}