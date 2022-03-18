using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraFollow == null) cameraFollow = GameObject.FindWithTag("Camera Look").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraFollow.forward);
    }
}
