using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabToPlace;
    private GameObject draggedImage;
    private RectTransform canvasTransform;

    public Vector2 spriteSize;

    private void Start() {
        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData eventData) {
        draggedImage = new GameObject("DraggedItem");
        draggedImage.transform.SetParent(canvasTransform);
        draggedImage.transform.SetAsLastSibling();
        
        var image = draggedImage.AddComponent<UnityEngine.UI.Image>();
        image.sprite = GetComponent<UnityEngine.UI.Image>().sprite;
        image.raycastTarget = false;
        
        var rt = draggedImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(spriteSize.x, spriteSize.y);
    }

    public void OnDrag(PointerEventData eventData) {
        if (draggedImage != null) 
            draggedImage.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (draggedImage != null)
            Destroy(draggedImage);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 worldPos = GetWorldMousePosition();
            Instantiate(prefabToPlace, worldPos, Quaternion.identity);
        }
    }

    private Vector3 GetWorldMousePosition() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = canvasTransform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}