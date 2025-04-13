using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [Header("All Panels")]
    public GameObject mainMenuPanel;
    public GameObject selectWorldPanel;
    public GameObject createWorldPanel;

    private Dictionary<string, GameObject> panels;

    private void Awake() {
        panels = new Dictionary<string, GameObject>
        {
            { "MainMenu", mainMenuPanel },
            { "SelectWorld", selectWorldPanel },
            { "CreateWorld", createWorldPanel }
        };
    }

    public void ShowPanel(string panelName) {
        foreach (var kvp in panels) {
            kvp.Value.SetActive(kvp.Key == panelName);
        }
    }
    
    public void ShowMainMenu() => ShowPanel("MainMenu");
    public void ShowSelectWorld() => ShowPanel("SelectWorld");
    public void ShowCreateWorld() => ShowPanel("CreateWorld");
}
