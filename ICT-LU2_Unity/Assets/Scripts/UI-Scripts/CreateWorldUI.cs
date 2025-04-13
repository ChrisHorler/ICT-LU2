using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateWorldUI : MonoBehaviour
{
    public WorldApiManager worldApiManager;
    
    public TMP_InputField nameInput;
    public TMP_InputField sizeXField;
    public TMP_InputField sizeYField;

    public TMP_Text errorText;

    public Button createButton;
    
    [Header("Scene Configuration")]
    public string worldSceneName = "Template-World";

    private void Start() {
        createButton.onClick.AddListener(OnCreatePressed);
        
        worldApiManager = FindObjectOfType<WorldApiManager>();
    }

    private void OnCreatePressed() {
        string worldName = nameInput.text;
        int sizeX = int.TryParse(sizeXField.text, out var x) ? x : -1;
        int sizeY = int.TryParse(sizeYField.text, out var y) ? y : -1;
        
        if (string.IsNullOrEmpty(worldName))
        {
            Debug.LogWarning("World name is empty.");
            errorText.text = "World name is empty.";
            return;
        }

        if (sizeX < 20 || sizeX > 200 || sizeY < 10 || sizeY > 100) {
            Debug.LogWarning("World size is out of range. (X: 20-200, Y: 10-100)");
            errorText.text = "World size is out of range. (X: 20-200, Y: 10-100)";
            return;
        }
        worldApiManager.CreateWorld(worldName, sizeX, sizeY);
        errorText.color = Color.white;
        errorText.text = "World has been created!";
    }
}
