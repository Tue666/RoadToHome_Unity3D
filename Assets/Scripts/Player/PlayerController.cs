using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller = null;

    private float cameraVertical = 0f;
    private float cameraSensitivity = 70f;
    private float gravity = -2f;
    private float velocity = 0f;
    private float jumpHeight = 0.2f;   
    private bool isBreathing = false;
    private bool isRunning = false;
    private bool isWalking = false;
    private float runningStaminaCost = 0.5f;
    private float walkingStaminaCost = 0.2f;
    private float staminaRestore = 1f;

    void InitializeIfNecessary()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeIfNecessary();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            UpdateCameraLook();
        UpdateMovement();
    }

    void UpdateCameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        if (mouseX == 0 && mouseY == 0) return;

        // Vertical lock
        cameraVertical -= mouseY;
        cameraVertical = Mathf.Clamp(cameraVertical, -90f, 90f);
        PlayerManager.Instance.cameraLook.transform.localEulerAngles = Vector3.right * cameraVertical;
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
        Vector3 move = (transform.right * direction.x + transform.forward * direction.y) + Vector3.up * velocity;
        controller.Move(move * (PlayerManager.Instance.movementSpeed + PlayerManager.Instance.movementPlus) * Time.deltaTime);
    }

    void HandleSpeedMovement()
    {
        if (PlayerManager.Instance.stamina <= 0)
        {
            StopMovement();
            return;
        }

        Breathing();

        if (Input.GetKey("left shift"))
        {
            if (!isRunning)
            {
                isRunning = true;
                PlayerManager.Instance.movementSpeed = 6f + PlayerManager.Instance.movementPlus;
                PlayerManager.Instance.StartStaminaDrop(-runningStaminaCost, new WaitForSeconds(0.1f));
                WeaponManager.currentAnimator.SetBool("Running", true);
            }
            return;
        }
        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            if (isRunning)
            {
                isRunning = false;
                isWalking = false; // Reset walking
                WeaponManager.currentAnimator.SetBool("Running", false);
            }
            if (!isWalking)
            {
                isWalking = true;
                PlayerManager.Instance.movementSpeed = 2f + PlayerManager.Instance.movementPlus;
                PlayerManager.Instance.StartStaminaDrop(-walkingStaminaCost, new WaitForSeconds(0.1f));
                WeaponManager.currentAnimator.SetBool("Walking", true);
            }
            return;
        }
        if (isWalking)
        {
            isWalking = false;
            PlayerManager.Instance.movementSpeed = 1f + PlayerManager.Instance.movementPlus;
            PlayerManager.Instance.StartStaminaDrop(staminaRestore, new WaitForSeconds(0.1f));
            WeaponManager.currentAnimator.SetBool("Walking", false);
        }
    }

    void StopMovement()
    {
        PlayerManager.Instance.movementSpeed = 0.5f;
        PlayerManager.Instance.StartStaminaDrop(staminaRestore, new WaitForSeconds(0.1f));
        WeaponManager.currentAnimator.SetBool("Running", false);
        WeaponManager.currentAnimator.SetBool("Walking", false);
    }

    void Breathing()
    {
        // Start breath when stamine less than 25
        if (PlayerManager.Instance.stamina <= 25)
        {
            if (!isBreathing)
            {
                SystemWindowManager.Instance.ShowWindowSystem(
                    "WARNING",
                    "Player about to run out of stamina, take a break!",
                    "TOP-RIGHT"
                );
                isBreathing = true;
                AudioManager.Instance.PlayMusic("Player Breath");
            }
        }
        else
        {
            if (isBreathing)
            {
                isBreathing = false;
                AudioManager.Instance.StopMusic();
            }
        }
    }
}
