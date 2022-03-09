using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private AudioClip breathClip = null;

    private float cameraVertical = 0f;
    private float cameraSensitivity = 120f;
    private float movementSpeed = 1.9f;
    private float gravity = -2f;
    private float velocity = 0f;
    private float jumpHeight = 0.2f;
    private bool lockCursor = true;
    private AudioSource audioSource;

    // Scripts
    private Switching switching;

    // Start is called before the first frame update
    void Start()
    {
        switching = FindObjectOfType<Switching>();
        controller = GetComponent<CharacterController>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = breathClip;
            audioSource.loop = true;
        }
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraLock();
        UpdateMovement();
    }

    void UpdateCameraLock()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

        // Vertical lock
        cameraVertical -= mouseY;
        cameraVertical = Mathf.Clamp(cameraVertical, -90f, 90f);
        playerCamera.localEulerAngles = Vector3.right * cameraVertical;
        // Horizontal lock
        transform.Rotate(Vector3.up * mouseX);
    }

    void UpdateMovement()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction.Normalize();

        if (controller.isGrounded)
        {
            velocity = 0f;
        }
        velocity += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        HandleSpeedMovement();
        switching.animator.SetFloat("Speed", movementSpeed);
        Vector3 move = (transform.right * direction.x + transform.forward * direction.y) + Vector3.up * velocity;
        controller.Move(move * movementSpeed * Time.deltaTime);
    }

    void HandleSpeedMovement()
    {
        if (Input.GetKey("left shift"))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            movementSpeed = 8f;
            return;
        }
        audioSource.Stop();
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            movementSpeed = 3f;
            return;
        }
        movementSpeed = 1.9f;
    }
}
