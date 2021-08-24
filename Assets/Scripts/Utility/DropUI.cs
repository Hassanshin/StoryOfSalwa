using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropUI : MonoBehaviour, IDropHandler
{
    public PileType pileType;

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log($"DROP : {eventData.pointerDrag} dropped to {this}");
    }
}

public enum PileType { bag, deck, trunk }