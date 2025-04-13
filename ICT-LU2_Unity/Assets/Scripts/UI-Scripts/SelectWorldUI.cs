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
            
            Button b = btnObj.GetComponent<Button>();
            b.onClick.AddListener(() => OnSelectWorldClicked(wId));
        }
    }

    private void OnSelectWorldClicked(int worldId)
    {
        Debug.Log("Selected world ID = " + worldId);
        worldApiManager.GetWorld(worldId);
    }
}
