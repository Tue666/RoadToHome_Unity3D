using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMuzzleFlash : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
