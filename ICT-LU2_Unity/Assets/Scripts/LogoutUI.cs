using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutUI : MonoBehaviour
{ 
    public void OnLogOutButton() {
        ApiManager.JwtToken = null;
        Debug.Log("User logged out, JWT cleared");
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
