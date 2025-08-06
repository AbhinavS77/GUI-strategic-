using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class PinManager : MonoBehaviour
{
    [Header("References")]
    public RectTransform mapContainer;
    public GameObject pinPrefab;
    public RectTransform pinsParent;
    public Camera uiCamera;
    public TileLoader tileLoader;

    private class PinData
    {
        public RectTransform rect;
        public double lon, lat;
    }
    private List<PinData> pins = new();

    void Awake()
    {
        if (pinsParent == null)
            pinsParent = mapContainer.Find("PinsParent") as RectTransform;
        if (pinsParent == null)
        {
            var go = new GameObject("PinsParent", typeof(RectTransform));
            go.transform.SetParent(mapContainer, false);
            pinsParent = go.GetComponent<RectTransform>();
            pinsParent.anchorMin = Vector2.zero;
            pinsParent.anchorMax = Vector2.one;
            pinsParent.sizeDelta = Vector2.zero;
            pinsParent.pivot = mapContainer.pivot;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mapContainer, Input.mousePosition, uiCamera, out Vector2 localPoint))
            {
                if (mapContainer.rect.Contains(localPoint))
                    PlacePin(localPoint);
            }
        }
    }

    void PlacePin(Vector2 localPoint)
    {
        // 1) Instantiate the pin
        var pinGO = Instantiate(pinPrefab, pinsParent);
        var pinRT = pinGO.GetComponent<RectTransform>();
        pinRT.pivot = new Vector2(0.5f, 1f);
        pinRT.anchoredPosition = localPoint;

        // 2) Compute normalized fraction across the map
        float mapW = mapContainer.rect.width;
        float mapH = mapContainer.rect.height;
        double fracX = (localPoint.x + mapW * 0.5) / mapW;
        double fracY = (mapH * 0.5 - localPoint.y) / mapH;

        // 3) Convert to lon/lat
        double lon = fracX * 360.0 - 180.0;
        double n = Math.PI - 2.0 * Math.PI * fracY;
        double lat = Math.Atan(Math.Sinh(n)) * 180.0 / Math.PI;

        pins.Add(new PinData { rect = pinRT, lon = lon, lat = lat });
        Debug.Log($"📌 Pin at lat={lat:F6}°, lon={lon:F6}° (zoom {tileLoader.currentZoom})");
    }
}
