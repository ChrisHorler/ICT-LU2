using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour {
    public ApiManager apiManager;

    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

    public TMP_Text titleText;
    
    public Button confirmButton;
    public TMP_Text confirmText;

    public Button switchButton;
    public TMP_Text switchText;

    [SerializeField] private TMP_Text errorText;
    
    private bool isLoginMode = true;

    
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        AutoAssignReferences();
        SetAuthMode(true);
        switchButton.onClick.AddListener(SwitchMode);
        confirmButton.onClick.AddListener(OnLoginButton);
    }

    public void SetAuthMode(bool loginMode) {
        isLoginMode = loginMode;

        if (loginMode) {
            titleText.text = "Login";
            confirmText.text = "Login";
            switchText.text = "Register Instead";
            
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnLoginButton);
        }
        else {
            titleText.text = "Register";
            confirmText.text = "Register";
            switchText.text = "Login Instead";
            
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnRegistrationButton);
        }
    }

    public void SwitchMode() {
        SetAuthMode(!isLoginMode);
        ShowError("");
    }
    
    public void OnRegistrationButton() {
        apiManager.RegisterUser(usernameField.text, passwordField.text);
    }

    public void OnLoginButton() {
        apiManager.LoginUser(usernameField.text, passwordField.text);
    }

    public void ShowError(string message) {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
    }
    
    private void AutoAssignReferences()
    {
        if (apiManager == null)
            apiManager = FindObjectOfType<ApiManager>();

        if (usernameField == null)
            usernameField = GameObject.Find("Username-Input")?.GetComponent<TMP_InputField>();

        if (passwordField == null)
            passwordField = GameObject.Find("Password-Input")?.GetComponent<TMP_InputField>();

        if (titleText == null)
            titleText = GameObject.Find("Title-Text")?.GetComponent<TMP_Text>();

        if (confirmButton == null)
            confirmButton = GameObject.Find("Confirmation-Button")?.GetComponent<Button>();

        if (confirmText == null && confirmButton != null)
            confirmText = confirmButton.GetComponentInChildren<TMP_Text>();

        if (switchButton == null)
            switchButton = GameObject.Find("Switch-Button")?.GetComponent<Button>();

        if (switchText == null && switchButton != null)
            switchText = switchButton.GetComponentInChildren<TMP_Text>();
    }
}
