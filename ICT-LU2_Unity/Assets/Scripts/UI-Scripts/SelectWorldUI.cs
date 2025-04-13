using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectWorldUI : MonoBehaviour
{
    public WorldApiManager worldApiManager;
    public Transform listParent;
    public GameObject worldButtonPrefab;

    private void OnEnable() {
        worldApiManager = FindObjectOfType<WorldApiManager>();
        worldApiManager.GetMyWorld();
    }

    public void PopulateWorldsList(List<int> worldIds, List<string> worldNames)
    {
        foreach (Transform child in listParent)
            Destroy(child.gameObject);

        for (int i = 0; i < worldIds.Count; i++)
        {
            int wId = worldIds[i];
            string wName = worldNames[i];

            GameObject btnObj = Instantiate(worldButtonPrefab, listParent);
            btnObj.GetComponentInChildren<TMP_Text>().text = wName;
            
            Button loadButton = btnObj.GetComponent<Button>();
            loadButton.onClick.AddListener(() => OnSelectWorldClicked(wId));
            
            Button deleteButton = btnObj.transform.Find("DeleteButton")?.GetComponent<Button>();
            if (deleteButton != null) {
                deleteButton.onClick.AddListener(() => ConfirmDeleteWorld(wId, wName));
            }
            else {
                Debug.LogWarning("Delete button not found in prefab!");
            }
        }
    }

    private void ConfirmDeleteWorld(int worldId, string name)
    {
        Debug.Log($"Deleting world {name} with ID: {worldId}");
        worldApiManager.DeleteWorld(worldId);

        Invoke(nameof(ReloadWorlds),1f);
    }
    
    private void OnSelectWorldClicked(int worldId)
    {
        Debug.Log("Selected world ID = " + worldId);
        worldApiManager.GetWorld(worldId);
    }

    private void ReloadWorlds()
    {
        worldApiManager.GetMyWorld();
    }
}
