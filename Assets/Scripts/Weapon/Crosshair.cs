using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float idleSize = 60f;
    [SerializeField] private float walkingSize = 75f;
    [SerializeField] private float runningSize = 100f;
    [SerializeField] private float jumpingSize = 150f;

    private float currentSize = 60f;
    private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (crosshair == null) crosshair = gameObject.GetComponent<RectTransform>();
        if (controller == null) controller = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ReSize();
    }

    void ReSize()
    {
        if (IsJumping) currentSize = Mathf.Lerp(currentSize, jumpingSize, speed * Time.deltaTime);
        else if (IsRunning) currentSize = Mathf.Lerp(currentSize, runningSize, speed * Time.deltaTime);
        else if (IsWalking) currentSize = Mathf.Lerp(currentSize, walkingSize, speed * Time.deltaTime);
        else currentSize = Mathf.Lerp(currentSize, idleSize, speed * Time.deltaTime);

        crosshair.sizeDelta = new Vector2(currentSize, currentSize);
    }

    bool IsJumping
    {
        get { return !controller.isGrounded; }
    }

    bool IsRunning
    {
        get { return Input.GetKey("left shift"); }
    }

    bool IsWalking
    {
        get { return (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && !IsRunning; }
    }
}
 
