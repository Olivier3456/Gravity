using UnityEngine;
using UnityEngine.UI;

public class TestShip : MonoBehaviour, IGravityReceiver
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float mass = 1f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image shipVisualImage;
    [SerializeField] private float thrustersForce = 1f;
    [SerializeField] private Vector3 startForce;


    private RectTransform uiParent;


    private bool up;
    private bool down;
    private bool left;
    private bool right;


    void Awake()
    {
        uiParent = shipVisualImage.rectTransform.parent as RectTransform;
    }


    void LateUpdate()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        // camera = null en Overlay ; sinon canvas.worldCamera
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiParent, screenPos, null, out Vector2 localPos))
        {
            shipVisualImage.rectTransform.anchoredPosition = localPos;
        }
    }


    void Start()
    {
        rb.useGravity = false;
        rb.mass = mass;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        if (startForce != Vector3.zero)
        {
            rb.AddForce(startForce, ForceMode.Impulse);
        }
    }


    void Update()
    {
        up = UnityEngine.InputSystem.Keyboard.current.wKey.isPressed;

        down = UnityEngine.InputSystem.Keyboard.current.sKey.isPressed;

        left = UnityEngine.InputSystem.Keyboard.current.aKey.isPressed;

        right = UnityEngine.InputSystem.Keyboard.current.dKey.isPressed;
    }


    void FixedUpdate()
    {
        if (up)
        {
            rb.AddForce(Vector3.up * thrustersForce, ForceMode.Force);
            Debug.Log("Thruster up.");
        }
        if (down)
        {
            rb.AddForce(Vector3.down * thrustersForce, ForceMode.Force);
            Debug.Log("Thruster down.");
        }
        if (left)
        {
            rb.AddForce(-Vector3.forward * thrustersForce, ForceMode.Force);
            Debug.Log("Thruster left.");
        }
        if (right)
        {
            rb.AddForce(Vector3.forward * thrustersForce, ForceMode.Force);
            Debug.Log("Thruster right.");
        }
    }


    // IGravityReceiver implementation.
    public void ApplyForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Force);
    }

    public float GetMass()
    {
        return mass;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
