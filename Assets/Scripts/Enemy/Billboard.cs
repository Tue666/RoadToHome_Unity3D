using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(transform.position + PlayerManager.Instance.cameraLook.transform.forward);
    }
}
