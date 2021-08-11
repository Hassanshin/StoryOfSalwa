using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class CardUI : DragAndDrop
{
    [SerializeField]
    private CardData data;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image border;

    [SerializeField]
    private TextMeshProUGUI cardName;

    [SerializeField]
    private TextMeshProUGUI damage;

    public void SetCardData(CardData _data)
    {
        data = _data;

        icon.color = Color.white;
        border.color = Color.white;

        cardName.text = data.name;
        icon.sprite = data.sprite;
    }

    public void SetBlank()
    {
        cardName.text = "";
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
}