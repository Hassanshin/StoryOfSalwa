using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : Singleton<MainMenuUI>
{
    public List<Canvas> panel = new List<Canvas>();
    public List<Image> ShopPanel = new List<Image>();

    [Header("Name")]
    [SerializeField]
    private GameObject NamePanel;

    [SerializeField]
    private TMP_InputField nameInput;

    [Header("Map")]
    [SerializeField]
    private LevelData[] levelDatas;

    [SerializeField]
    private Button[] levelButtons;

    public override void Initialization()
    {
        nameInput.onEndEdit.AddListener(SetName);
        NamePanel.SetActive(SaveManager.Instance.IsNewPlayer);
        setupButtons();
    }

    private void setupButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int copy = i;
            levelButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelDatas[i].name;
            levelButtons[i].onClick.AddListener(() =>
            {
                GameManager.Instance.Level.SetLevelData(levelDatas[copy]);
                GameManager.Instance.StarGame();
            });
        }
    }

    public void setVideoPanel(bool state)
    {
        panel[2].gameObject.SetActive(state);
    }

    public void PlayGame()
    {
        //GameManager.Instance.StarGame();
    }

    public void BackToMain()
    {
        panel[0].gameObject.SetActive(true);
        panel[1].gameObject.SetActive(false);
        GameManager.Instance.BackToMain();
    }

    public void HideAllShopPanel()
    {
        for (int i = 0; i < ShopPanel.Count; i++)
        {
            ShopPanel[i].gameObject.SetActive(false);
        }
    }

    #region name

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

    #endregion name
}