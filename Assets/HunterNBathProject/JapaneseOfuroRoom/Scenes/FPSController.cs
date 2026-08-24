using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public Camera playerCamera;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 89f;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player left/right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        playerCamera.transform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement =
            transform.right * x +
            transform.forward * z;

        movement = Vector3.ClampMagnitude(movement, 1f);

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = movement * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}