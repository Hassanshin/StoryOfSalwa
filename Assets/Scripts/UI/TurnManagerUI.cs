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

    public IEnumerator generateCharTurn(int totalChars)
    {
        sliderHeight = slider.rect.height;

        for (int i = 0; i < totalChars; i++)
        {
            TurnCharUI _spawn = Instantiate(charTurnPrefab, parentCharTurn).GetComponent<TurnCharUI>();
            _spawn.data = TurnManager.Instance.Characters[i];

            // set UI as characters

            turnCharUIs.Add(_spawn);
            setPos(_spawn, _spawn.data.Speed);
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
        float targetY = movePercentage * sliderHeight / 100;

        ui.rect.anchoredPosition = new Vector3(0, -targetY, 0);
    }

    private float getDelta(TurnCharUI ui, float movePercentage)
    {
        float targetY = movePercentage * sliderHeight / 100;
        float delta = Mathf.Abs(ui.curYPos) - targetY;

        return Mathf.Abs(delta);
    }

    private void movePos(TurnCharUI ui, float amount)
    {
        float targetY = (ui.curYPos - amount + 1) % -sliderHeight;

        // start from 0 if higher than sliderLength
        if (ui.curYPos - amount + 1 < -sliderHeight)
        {
            ui.rect.anchoredPosition = Vector3.zero;
        }
        LeanTween.moveY(ui.rect, targetY, 1f).setEase(LeanTweenType.easeOutQuint);
    }

    public IEnumerator MoveToReady(BaseChar charReady)
    {
        float delta = getDelta(findCharUi(charReady), 100);
        yield return moveAll(delta);

        yield return null;
    }

    private IEnumerator moveAll(float delta)
    {
        foreach (TurnCharUI item in turnCharUIs)
        {
            movePos(item, delta);
        }
        yield return new WaitForSeconds(1f);
        yield return null;
    }

    private TurnCharUI findCharUi(BaseChar character)
    {
        return turnCharUIs.Find(a => a.data == character);
    }
}