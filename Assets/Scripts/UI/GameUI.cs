using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    public Canvas Canvas;

    [Header("GameOverPanel")]
    [SerializeField]
    private Canvas gameOverCanvas;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    public CardDetailHandler cardDetail;

    public override void Initialization()
    {
        GameManager.Instance.Level.OnGameOver.AddListener(showGameOverPanel);
        GameManager.Instance.OnBackToMenu.AddListener(resetPanel);
    }

    private void resetPanel()
    {
        gameOverCanvas.gameObject.SetActive(!true);
    }

    private void showGameOverPanel(bool isWin)
    {
        gameOverCanvas.gameObject.SetActive(true);
        gameOverText.text = isWin ? "You Win!" : "You Lose";
    }
}