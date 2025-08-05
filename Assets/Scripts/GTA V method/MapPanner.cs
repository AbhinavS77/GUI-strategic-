//using UnityEngine;
//using UnityEngine.InputSystem;

//public class MapPanner : MonoBehaviour
//{
//    public RectTransform mapContainer;
//    public float panSpeed = 1f;

//    private MapControls controls;
//    private bool isDragging = false;
//    private Vector2 lastMousePosition;

//    private void Awake()
//    {
//        controls = new MapControls();
//        controls.Map.Drag.started += ctx => StartDragging();
//        controls.Map.Drag.canceled += ctx => StopDragging();
//    }

//    private void OnEnable() => controls.Enable();
//    private void OnDisable() => controls.Disable();

//    private void StartDragging()
//    {
//        isDragging = true;
//        lastMousePosition = Mouse.current.position.ReadValue();
//    }

//    private void StopDragging() => isDragging = false;

//    private void Update()
//    {
//        if (isDragging)
//        {
//            Vector2 currentMouse = Mouse.current.position.ReadValue();
//            Vector2 delta = currentMouse - lastMousePosition;
//            mapContainer.anchoredPosition += delta * panSpeed;
//            lastMousePosition = currentMouse;
//        }
//    }
//}


using UnityEngine;
using UnityEngine.InputSystem;

public class MapPanner : MonoBehaviour
{
    [Header("References")]
    public RectTransform mapContainer;
    public TileLoader tileLoader;   // Assign your TileLoader here

    [Header("Pan Settings")]
    public float panSpeed = 1f;

    private MapControls controls;
    private bool isDragging = false;
    private Vector2 lastMousePosition;

    private void Awake()
    {
        controls = new MapControls();
        controls.Map.Drag.started += ctx => StartDragging();
        controls.Map.Drag.canceled += ctx => StopDragging();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void StartDragging()
    {
        isDragging = true;
        lastMousePosition = Mouse.current.position.ReadValue();
    }

    private void StopDragging() => isDragging = false;

    private void Update()
    {
        if (!isDragging) return;

        Vector2 currentMouse = Mouse.current.position.ReadValue();
        Vector2 delta = currentMouse - lastMousePosition;
        lastMousePosition = currentMouse;

        // Apply pan
        mapContainer.anchoredPosition += delta * panSpeed;

        // Clamp to bounds using TileLoader’s logic
        if (tileLoader != null)
            tileLoader.ClampMap();
    }
}
