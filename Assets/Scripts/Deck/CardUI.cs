using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : DragAndDrop
{
    [SerializeField]
    private CardData data;

    public void SetCardData(CardData _data)
    {
        data = _data;
    }

    internal override void Start()
    {
        base.Start();
    }

    public override void OnDropActionWorld()
    {
        base.OnDropActionWorld();

        // get target in parent
        Transform target = RaycastHandler.Instance.RayObject;

        if (target != null && data != null)
        {
            data.Action(target.parent.GetComponent<BaseChar>());
        }
    }
}