using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogManager : Singleton<DialogManager>
{
    public DialogData curDialog;

    public UnityEvent OnDialogDone;

    private Coroutine dialogCoroutine;

    private bool next;

    [Header("UI")]
    [SerializeField]
    private Canvas dialogPanel;

    [SerializeField]
    private Image dialogBg;

    [SerializeField]
    private TextMeshProUGUI dialogText;

    [SerializeField]
    private Image speakerIcon;

    [SerializeField]
    private TextMeshProUGUI speakerText;

    [SerializeField]
    private Button nextBtn;

    public override void Initialization()
    {
        nextBtn.onClick.AddListener(NextDialog);
        OnDialogDone.AddListener(() =>
        {
            Debug.Log("Dialog Done");
        });

        GameManager.Instance.OnBackToMenu.AddListener(Clear);
    }

    public void StartDialog(DialogData cur)
    {
        curDialog = cur;
        dialogPanel.gameObject.SetActive(true);
    }

    public IEnumerator Dialoging()
    {
        dialogBg.sprite = curDialog.Background;
        for (int i = 0; i < curDialog.Dialog.Length; i++)
        {
            next = false;
            DialogText dial = curDialog.Dialog[i];

            dialogText.text = dial.Text;
            speakerText.text = dial.Speaker.name;
            speakerIcon.sprite = dial.Speaker.icon;

            if (curDialog.LeftSpeaker == dial.Speaker)
            {
                // left pane
            }
            yield return new WaitUntil(() => next);
        }

        yield return DialogDone();
    }

    public void NextDialog()
    {
        next = true;
    }

    private IEnumerator DialogDone()
    {
        OnDialogDone?.Invoke();
        dialogPanel.gameObject.SetActive(false);

        yield return null;
    }

    private void Clear()
    {
        //StopCoroutine(dialogCoroutine);
    }
}