using UnityEngine;

public class CameraEdgeMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    [Range(1f, 100f)] public float borderThickness = 10f;

    [Header("Optional Limits")]
    public bool useLimits = false;
    public Vector2 minLimit;
    public Vector2 maxLimit;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // Get mouse position
        Vector3 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 direction = Vector3.zero;

        // Horizontal movement
        if (mousePos.x <= borderThickness)
            direction.x = -1;
        else if (mousePos.x >= screenWidth - borderThickness)
            direction.x = 1;

        // Vertical movement
        if (mousePos.y <= borderThickness)
            direction.y = -1;
        else if (mousePos.y >= screenHeight - borderThickness)
            direction.y = 1;

        // Apply movement
        pos += direction.normalized * moveSpeed * Time.deltaTime;

        // Clamp camera inside limits if needed
        if (useLimits)
        {
            pos.x = Mathf.Clamp(pos.x, minLimit.x, maxLimit.x);
            pos.y = Mathf.Clamp(pos.y, minLimit.y, maxLimit.y);
        }

        transform.position = pos;
    }
}