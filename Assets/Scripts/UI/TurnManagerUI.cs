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

    public IEnumerator generateCharTurn(List<BaseChar> baseChar)
    {
        sliderHeight = slider.rect.height;

        for (int i = 0; i < baseChar.Count; i++)
        {
            TurnCharUI _spawn = Instantiate(charTurnPrefab, parentCharTurn).GetComponent<TurnCharUI>();
            _spawn.Data = baseChar[i];

            turnCharUIs.Add(_spawn);
            _spawn.transform.SetAsFirstSibling();
            setPos(_spawn, _spawn.Data.s_Speed.CurValue);
        }

        yield return null;
    }

    public void RearrangeUi()
    {
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

        float targetY = movePercentage * -sliderHeight / 100;

        ui.rect.anchoredPosition = Vector3.zero;

        LeanTween.moveY(ui.rect, targetY, 1f).setEase(LeanTweenType.easeOutQuint);
    }

    public void MovePosEffect(BaseChar ui, float amount)
    {
        movePos(FindCharUi(ui), amount);
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
        curReady.Data.TurnPlayed = TurnManager.Instance.TurnCount;

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
            setPos(curReady, 0);
        }
        curReady = null;
    }

    public TurnCharUI FindCharUi(BaseChar character)
    {
        return turnCharUIs.Find(a => a.Data == character);
    }
}