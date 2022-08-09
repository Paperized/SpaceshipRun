using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedCamera : MonoBehaviour
{
    private float desiredWidth;

    void Start()
    {
        Camera _camera = GetComponent<Camera>();
        desiredWidth = MapManager.Instance.bgWidth;

        float unitsPerPixel = desiredWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        _camera.orthographicSize = desiredHalfHeight;
    }
}
