using System.Collections;
using UnityEngine;

public class Scope : MonoBehaviour
{
    [SerializeField] private GameObject scopeOverlay;
    [SerializeField] private GameObject weaponCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float scopeFOV = 10f;
    [SerializeField] private float openSpeed = 5f;

    private bool isScoped = false;
    private float previousFOV;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        weaponCamera = GameObject.FindWithTag("WeaponCamera");
        mainCamera = GameObject.FindWithTag("Camera").GetComponent<Camera>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(OnScoped());
        }
        if (Input.GetButtonUp("Fire2"))
        {
            StartCoroutine(OnUnScoped());
        }
    }

    IEnumerator OnUnScoped()
    {
        isScoped = false;
        weaponCamera.SetActive(true);
        scopeOverlay.SetActive(false);
        animator.SetBool("Scoped", false);

        mainCamera.fieldOfView = previousFOV;
        while (mainCamera.fieldOfView < previousFOV)
        {
            mainCamera.fieldOfView = mainCamera.fieldOfView + openSpeed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator OnScoped()
    {
        isScoped = true;
        animator.SetBool("Scoped", true);
        yield return new WaitForSeconds(0.23f);
        if (isScoped)
        {
            weaponCamera.SetActive(false);
            scopeOverlay.SetActive(true);

            previousFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = scopeFOV;
            while (mainCamera.fieldOfView > scopeFOV)
            {
                mainCamera.fieldOfView = mainCamera.fieldOfView - openSpeed;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
