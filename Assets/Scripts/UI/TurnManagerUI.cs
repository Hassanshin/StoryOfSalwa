using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManagerUI : MonoBehaviour
{
    private float sliderHeight;

    [SerializeField]
    private List<TurnCharUI> turnCharUIs = new List<TurnCharUI>();

    [SerializeField]
    private GameObject charTurnPrefab;

    [Header("Components")]
    [SerializeField]
    private Transform parentCharTurn;

    [SerializeField]
    private RectTransform slider;

    private TurnCharUI curReady;

    public IEnumerator generateCharTurn(int totalChars)
    {
        sliderHeight = slider.rect.height;

        for (int i = 0; i < totalChars; i++)
        {
            TurnCharUI _spawn = Instantiate(charTurnPrefab, parentCharTurn).GetComponent<TurnCharUI>();
            _spawn.data = TurnManager.Instance.Characters[i];

            turnCharUIs.Add(_spawn);
            _spawn.transform.SetAsFirstSibling();
            setPos(_spawn, _spawn.data.s_Speed.Value);
        }

        yield return null;
    }

    public void resetUiTurn()
    {
        foreach (TurnCharUI item in turnCharUIs)
        {
            Destroy(item.gameObject);
        }
        turnCharUIs.Clear();
    }

    private void setPos(TurnCharUI ui, float movePercentage)
    {
        ui.PosPercent = Mathf.Round(movePercentage * 100f) / 100f;

        float targetY = movePercentage * sliderHeight / 100;

        ui.rect.anchoredPosition = new Vector3(0, -targetY, 0);
    }

    private void movePos(TurnCharUI ui, float amount)
    {
        ui.PosPercent += amount;

        float targetY = ui.PosPercent * -sliderHeight / 100;
        targetY = Mathf.Clamp(targetY, -sliderHeight, 0);

        LeanTween.moveY(ui.rect, targetY, 1f).setEase(LeanTweenType.easeOutQuint);
    }

    public IEnumerator MoveToReady(BaseChar charReady)
    {
        curReady = FindCharUi(charReady);
        curReady.TurnPlayed = TurnManager.Instance.TurnCount;

        float delta = 100 - curReady.PosPercent;

        foreach (TurnCharUI item in turnCharUIs)
        {
            movePos(item, delta);
        }
        yield return new WaitForSeconds(1f);
    }

    public void MoveToTop()
    {
        if (curReady != null)
        {
            // not working using this method
            setPos(curReady, 0);
        }
        curReady = null;
    }

    public TurnCharUI FindCharUi(BaseChar character)
    {
        return turnCharUIs.Find(a => a.data == character);
    }
}