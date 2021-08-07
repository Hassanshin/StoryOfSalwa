using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : DragAndDrop
{
    internal override void Start()
    {
        base.Start();
    }

    public override void OnDropActionWorld()
    {
        base.OnDropActionWorld();

        Transform target = RaycastHandler.Instance.RayObject;
        if (target != null)
            Debug.Log($"attacking {target.parent.name}");
    }
}