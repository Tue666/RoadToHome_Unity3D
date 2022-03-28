using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform cameraFollow;

    void InitializeIfNecessary()
    {
        if (cameraFollow == null) cameraFollow = GameObject.FindWithTag("Camera Look").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeIfNecessary();
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraFollow.forward);
    }
}
