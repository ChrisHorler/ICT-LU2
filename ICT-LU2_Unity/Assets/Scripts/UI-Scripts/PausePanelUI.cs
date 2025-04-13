using UnityEngine;

public class PausePanelUI : MonoBehaviour
{
    public GameObject pausePanel;

    private bool paused = false;
    
    public void TogglePause() {
        paused = !paused;

        if (paused) {
            pausePanel.SetActive(true);
        }
        else {
            pausePanel.SetActive(false);
        }
    }
}
