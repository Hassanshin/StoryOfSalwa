using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class CardUI : DragAndDrop
{
    [SerializeField]
    private CardData data;
    public CardData Data => data;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image border;

    [SerializeField]
    private TextMeshProUGUI cardName;

    [SerializeField]
    private TextMeshProUGUI damage;

    [SerializeField]
    private Button cardBtn;

    public void SetCardData(CardData _data)
    {
        data = _data;

        cardBtn.onClick.RemoveAllListeners();

        icon.color = Color.white;
        border.color = Color.white;

        cardName.text = data.name;
        icon.sprite = data.sprite;

        if (data is CardDataAtk)
        {
            CardDataAtk atkData = (CardDataAtk)data;
            damage.text = $"{ atkData.totalDamage}";
        }
        else
        {
            CardDataSup atkData = (CardDataSup)data;
            damage.text = $"{ atkData.healAmount}";
        }

        cardBtn.onClick.RemoveAllListeners();
        cardBtn.onClick.AddListener(() =>
        {
            if (data != null)
                CardDetailHandler.Instance.ViewData = data;
        });
    }

    public void SetBlank(string blankText = "")
    {
        cardName.text = blankText;

        data = null;

        icon.color = new Color(0, 0, 0, 0);
        border.color = new Color(0, 0, 0, 0);
    }

    internal override void Start()
    {
        base.Start();
    }

    public override void OnDropActionWorld()
    {
        base.OnDropActionWorld();

        Transform target = RaycastHandler.Instance.RayObject;

        if (target != null && data != null)
        {
            // get target in parent
            GameManager.Instance.Level.Player.Attacking(target.parent.GetComponent<BaseChar>(), data);
        }
    }

    public override void OnDropAlike(DragAndDrop _draggedSlot)
    {
        base.OnDropAlike(_draggedSlot);

        if (_draggedSlot != null && _draggedSlot.TryGetComponent(out CardUI ui))
            Inventory.Instance.Swap(this, _draggedSlot.GetComponent<CardUI>());
    }
}