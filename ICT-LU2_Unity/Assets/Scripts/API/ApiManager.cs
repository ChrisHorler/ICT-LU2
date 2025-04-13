using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ApiManager : MonoBehaviour
{
    private static ApiManager instance;
    
    public AuthUI authUI;
    public string currentJwtToken;
    
    [Header("API Configuration")] 
    [Tooltip("Base URL of the live API")]
    
    public string baseApiUrl = "https://avansict2227807.azurewebsites.net";
    public static string JwtToken;

    [Serializable]
    private class UserDto {
        public string username;
        public string password;
    }

    [Serializable]
    public class LoginResponse {
        public string token;
    }

    [Serializable]
    private class WorldDto {
        public string name;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // ======================
    // REGISTER
    // ======================
    
    /// <summary>
    /// Registers a new user with the API.
    /// </summary>
    /// <param name="username">Desired username</param>
    /// <param name="password">Desired password</param>
    public void RegisterUser(string username, string password) {
        StartCoroutine(RegisterCoroutine(username, password));
    }

    private IEnumerator RegisterCoroutine(string username, string password) {
        string endpoint = $"{baseApiUrl}/api/auth/register";
        
        var user = new UserDto { username = username, password = password };
        string jsonBody = JsonUtility.ToJson(user);
        
        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log($"Register Response: {request.downloadHandler.text}");
            authUI.SetAuthMode(true);
        }
        else {
            Debug.LogError($"Registration error: {request.error} | {request.downloadHandler.text}");
        }
    }
    
    // ======================
    // LOGIN
    // ======================
    
    /// <summary>
    /// Logs a user in, retrieving a JWT on success
    /// </summary>
    /// <param name="username">User's username</param>
    /// <param name="password">User's password</param>
    public void LoginUser(string username, string password) {
        StartCoroutine(LoginCoroutine(username,password));
    }

    private IEnumerator LoginCoroutine(string username, string password) {
        string endpoint = $"{baseApiUrl}/api/auth/login";
        var user = new UserDto { username = username, password = password };
        string jsonBody = JsonUtility.ToJson(user);
        
        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            string rawResponse = request.downloadHandler.text;
            Debug.Log($"Login response: {rawResponse}");

            try {
                LoginResponse data = JsonUtility.FromJson<LoginResponse>(rawResponse);
                if (!string.IsNullOrEmpty(data.token)) {
                    JwtToken = data.token;
                    Debug.Log($"JWT Token Stored: {data.token}");
                    currentJwtToken = JwtToken;
                    SceneManager.LoadScene(sceneBuildIndex: 1);
                }
                else {
                    Debug.LogError("No token found in login response");
                }
            }
            catch (Exception e) {
                Debug.LogError($"Failed to parse login response: {e.Message}");
            }
        }
        else {
            Debug.LogError($"Login error: {request.error} | {request.downloadHandler.text}");
        }
    }
    
    
    // ======================
    // CREATE WORLD
    // ======================

    /// <summary>
    /// Create a new "World" using the API. Requires a valid JWT token.
    /// </summary>
    /// <param name="worldName">Name of the World</param>
    public void CreateWorld(string worldName) {
        StartCoroutine(CreateWorldCoroutine(worldName));
    }
    private IEnumerator CreateWorldCoroutine(string worldName) {
        string endpoint = $"{baseApiUrl}/api/world";
        
        var worldDto = new WorldDto  { name = worldName };
        string jsonBody = JsonUtility.ToJson(worldDto);
        
        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        
        request.SetRequestHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(JwtToken)) {
            request.SetRequestHeader("Authorization", "Bearer " + JwtToken);
        }
        else {
            Debug.LogWarning("No JWT Token set. This request will likely fail");
        }
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log($"Create World Response: {request.downloadHandler.text}");
        }
        else {
            Debug.LogError($"CreateWorld error: {request.error} | {request.downloadHandler.text}");
        }
    }
    
    // ======================
    // DELETE WORLD
    // ======================

    /// <summary>
    /// Deletes a world by its ID. Requires the JWT token.
    /// </summary>
    /// <param name="worldId"></param>
    public void DeleteWorld(int worldId) {
        StartCoroutine(DeleteWorldCoroutine(worldId));
    }
    private IEnumerator DeleteWorldCoroutine(int worldId) {
        string endpoint = $"{baseApiUrl}/api/world/{worldId}";
        UnityWebRequest request = new UnityWebRequest(endpoint);
        request.downloadHandler = new DownloadHandlerBuffer();

        if (!string.IsNullOrEmpty(JwtToken)) {
            request.SetRequestHeader("Authorization", "Bearer " + JwtToken);
        }
        else {
            Debug.LogWarning("No JWT Token set. This request will likely fail");
        }
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log($"Delete World Response: {request.downloadHandler.text}");
        }
        else {
            Debug.LogError($"DeleteWorld error: {request.error} | {request.downloadHandler.text}");
        }
    }
}