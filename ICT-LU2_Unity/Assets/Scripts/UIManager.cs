using UnityEngine;
public class UIManager : MonoBehaviour
{
    public RectTransform menuPanel;
    public Vector2 hiddenPosition;
    public Vector2 visiblePosition;
    public float slideSpeed = 5f;

    private bool isVisible = false;
    private Coroutine slideRoutine;

    public void ToggleMenu() {
        isVisible = !isVisible;

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);
        
        slideRoutine = StartCoroutine(SlideMenu(isVisible ? visiblePosition : hiddenPosition));
    }
    private System.Collections.IEnumerator SlideMenu(Vector2 targetPos) {
        while (Vector2.Distance(menuPanel.anchoredPosition, targetPos) > 0.01f) {
            menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, targetPos, slideSpeed * Time.deltaTime);
            yield return null;
        }
        menuPanel.anchoredPosition = targetPos;
    }
}