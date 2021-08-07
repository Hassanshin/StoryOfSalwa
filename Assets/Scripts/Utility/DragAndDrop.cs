using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [Tooltip("should reference the canvas scaler to drag and drop")]
    private Canvas canvas;

    internal Image draggedImage;
    internal RectTransform rect;
    internal CanvasGroup canvasGroup;

    private bool dropToWorld
    {
        get => !RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition);
    }

    internal virtual void Start()
    {
        canvas = GameUI.Instance.Canvas;

        draggedImage = transform.GetChild(0).GetComponent<Image>();
        rect = draggedImage.GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    #region drag n drop

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        rect.anchoredPosition = Vector3.zero;
        draggedImage.raycastTarget = true;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (dropToWorld)
        {
            OnDropActionWorld();
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        draggedImage.raycastTarget = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        OnDropActionUI(eventData.pointerDrag.GetComponent<DragAndDrop>());
    }

    public virtual void OnDropActionUI(DragAndDrop _draggedSlot)
    {
#if UNITY_EDITOR
        //Debug.Log($"{this} dragged to {_draggedSlot}");
#endif
    }

    public virtual void OnDropActionWorld()
    {
#if UNITY_EDITOR
        //Debug.Log($"{this} dragged to WORLD");
#endif
    }

    #endregion drag n drop
}