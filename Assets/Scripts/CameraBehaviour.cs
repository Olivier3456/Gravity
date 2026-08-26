using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField] private float cameraPositionSpeed = 1f;
    [SerializeField] private float cameraScrollSpeed = 1f;

    void Start()
    {

    }

    void Update()
    {
        // var delta = Mouse.current.delta;
        float mouseDeltaX = Mouse.current.delta.x.value;
        float mouseDeltaY = Mouse.current.delta.y.value;

        Camera.main.transform.position += cameraPositionSpeed * new Vector3(0f, mouseDeltaY, mouseDeltaX);

        Camera.main.orthographicSize += cameraScrollSpeed * Mouse.current.scroll.value.y;
        // Debug.Log(Mouse.current.scroll.value);
    }
}
