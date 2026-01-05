using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public float verticalLookLimit = 80f;

    [Header("Sprint")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Head Bob")]
    public float bobFrequency = 2f;
    public float bobAmplitude = 0.05f;

    private CharacterController controller;
    private float verticalVelocity;
    private float xRotation = 0f;
    private bool isSprinting;
    private float bobTimer = 0f;
    private Vector3 cameraDefaultLocalPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraDefaultLocalPos = cameraTransform.localPosition;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleHeadBob();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (player body)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Keeps player grounded
        }

        verticalVelocity += gravity * Time.deltaTime;

        isSprinting = Input.GetKey(sprintKey) && controller.isGrounded;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 velocity = move * currentSpeed;

        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
    void HandleHeadBob()
    {
        if (!controller.isGrounded)
            return;

        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        if (horizontalVelocity.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            cameraTransform.localPosition = new Vector3(
                cameraDefaultLocalPos.x,
                cameraDefaultLocalPos.y + bobOffset,
                cameraDefaultLocalPos.z
            );
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                cameraDefaultLocalPos,
                Time.deltaTime * 5f
            );
        }
    }
}
