using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Sprite sprite;
    public string typeName;
    public BuildingSystem buildingSystem;

    private GameObject dragPreview;
    private RectTransform dragPreviewRect;
    private Canvas parentCanvas;

    private void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create preview icon
        dragPreview = new GameObject("DragPreview");
        dragPreview.transform.SetParent(parentCanvas.transform, false);
        dragPreview.transform.SetAsLastSibling();

        Image image = dragPreview.AddComponent<Image>();
        image.sprite = sprite;
        image.raycastTarget = false;

        dragPreviewRect = dragPreview.GetComponent<RectTransform>();
        dragPreviewRect.sizeDelta = new Vector2(100, 100);
        dragPreviewRect.pivot = new Vector2(0.5f, 0.5f);

        UpdatePreviewPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdatePreviewPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragPreview != null)
            Destroy(dragPreview);

        // Only place if not over UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            
            buildingSystem.CreateLocalObjectAndSave(worldPos, typeName, sprite);
        }
    }

    private void UpdatePreviewPosition()
    {
        if (dragPreviewRect == null) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition,
            parentCanvas.worldCamera,
            out pos
        );

        dragPreviewRect.localPosition = pos;
    }
}
