using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    public WorldApiManager worldApiManager;
    
    [System.Serializable]
    public class BuildOption {
        public string typeName;
        public Sprite sprite;
    }
    
    [Header("Building Options")]
    public BuildOption[] buildOptions;
    public GameObject buildingPrefab;

    [Header("UI")]
    public Transform buttonParent;
    public GameObject buttonTemplate;
    
    private Sprite selectedSprite;
    private string selectedTypeName;

    private void Start() {
        PopulateBuildingMenu();
        
        worldApiManager = FindObjectOfType<WorldApiManager>();
    }

    private void Update() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (Input.GetMouseButtonDown(0) && selectedSprite != null) {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            
            GameObject obj = Instantiate(buildingPrefab, worldPos, Quaternion.identity);
            obj.GetComponent<SpriteRenderer>().sprite = selectedSprite;
        }
    }

    void PopulateBuildingMenu()
    {
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);

        foreach (var option in buildOptions)
        {
            GameObject btnObject = Instantiate(buttonTemplate, buttonParent);

            Transform iconTransform = btnObject.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null)
                    iconImage.sprite = option.sprite;
            }

            Button btn = btnObject.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                selectedSprite = option.sprite;
                Debug.Log("Selected: " + option.sprite);
            });
            
            DraggableUI draggable = btnObject.AddComponent<DraggableUI>();
            draggable.buildingSystem = this;
            draggable.sprite = option.sprite;
            draggable.typeName = option.typeName;
        }
    }

    public void CreateLocalObjectAndSave(Vector3 position, string typeName, Sprite sprite)
    {
        GameObject obj = Instantiate(buildingPrefab, position, Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
        
        int currentWorldId = TempWorldDataHolder.CurrentWorldId;
        worldApiManager.AddObjectToWorld(currentWorldId, typeName, position.x, position.y, 0f, 0.5f);
        Debug.Log($"Uploaded Object: {typeName} Position: {position} to World Id: {currentWorldId}");
        
    }

}
