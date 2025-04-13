using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 10f;

    [Header("World Settings")]
    public GameObject backgroundWorld;

    private float minX, maxX, minY, maxY;
    private Camera cam;

    private void Start() {
        cam = Camera.main;
        
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        
        Vector3 bgScale = backgroundWorld.transform.localScale;
        Vector3 bgPosition = backgroundWorld.transform.position;

        float halfWidth = camWidth / 2f;
        float halfHeight = camHeight / 2f;

        float worldHalfWidth = bgScale.x / 2f;
        float worldHalfHeight = bgScale.y / 2f;

        minX = bgPosition.x - worldHalfWidth + halfWidth;
        maxX = bgPosition.x + worldHalfWidth - halfWidth;

        minY = bgPosition.y - worldHalfHeight + halfHeight;
        maxY = bgPosition.y + worldHalfHeight - halfHeight;

        Debug.Log($"Camera Clamp: X({minX}, {maxX}) Y({minY}, {maxY})");
    }

    private void Update() {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) move.y += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) move.y -= 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move.x -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move.x += 1f;

        move = move.normalized * (movementSpeed * Time.deltaTime);
        transform.position += move;

        // Clamp the camera position to world limits
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, minX, maxX);
        clamped.y = Mathf.Clamp(clamped.y, minY, maxY);
        transform.position = clamped;
    }
}