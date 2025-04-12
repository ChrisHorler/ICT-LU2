using System;
using TMPro;
using UnityEngine;
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

    private bool isLoginMode = true;

    private void Start() {
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
    }
    
    public void OnRegistrationButton() {
        apiManager.RegisterUser(usernameField.text, passwordField.text);
    }

    public void OnLoginButton() {
        apiManager.LoginUser(usernameField.text, passwordField.text);
    }
}
