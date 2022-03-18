using System.Collections;
using UnityEngine;

public class Scope : MonoBehaviour
{
    [SerializeField] private Camera cameraFOV;
    [SerializeField] private Switching switching;

    private int[] scopedFOV = { 10, 0, 0, 0, 0, 0, 0, 0, 20 }; // FOV from no scope to scope x8
    private bool isScoped = false;
    private float previousFOV;
    private Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        if (switching == null) switching = gameObject.GetComponent<Switching>();
        if (cameraFOV == null) cameraFOV = GameObject.FindWithTag("Camera Recoil").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && gun != null)
        {
            StartCoroutine(OnScoped());
        }
        if (Input.GetButtonUp("Fire2") && gun != null)
        {
             StartCoroutine(OnUnScoped());
        }
    }

    IEnumerator OnUnScoped()
    {
        isScoped = false;
        switching.animator.SetBool("Scoped", false);

        if (previousFOV <= 0) yield break;
        while (cameraFOV.fieldOfView < previousFOV)
        {
            cameraFOV.fieldOfView = cameraFOV.fieldOfView + gun.zoomSpeed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator OnScoped()
    {
        isScoped = true;
        switching.animator.SetBool("Scoped", true);

        if (!isScoped) yield break;

        previousFOV = cameraFOV.fieldOfView;
        float newFOV = cameraFOV.fieldOfView - scopedFOV[gun.scopeEquipped];
        while (cameraFOV.fieldOfView > newFOV)
        {
            cameraFOV.fieldOfView = cameraFOV.fieldOfView - gun.zoomSpeed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Switch()
    {
        gun = gameObject.GetComponentInChildren<Gun>();
    }
}


//using System.Collections;
//using UnityEngine;

//public class Scope : MonoBehaviour
//{
//    [SerializeField] private GameObject scopeOverlay;
//    [SerializeField] private GameObject weaponCamera;
//    [SerializeField] private Camera mainCamera;
//    [SerializeField] private float scopeFOV = 10f;
//    [SerializeField] private float openSpeed = 5f;

//    private bool isScoped = false;
//    private float previousFOV;
//    private Switching switching;

//    // Start is called before the first frame update
//    void Start()
//    {
//        weaponCamera = GameObject.FindWithTag("WeaponCamera");
//        mainCamera = GameObject.FindWithTag("Camera").GetComponent<Camera>();
//        switching = GameObject.FindObjectOfType<Switching>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetButtonDown("Fire2"))
//        {
//            StartCoroutine(OnScoped());
//        }
//        if (Input.GetButtonUp("Fire2"))
//        {
//            StartCoroutine(OnUnScoped());
//        }
//    }

//    IEnumerator OnUnScoped()
//    {
//        isScoped = false;
//        weaponCamera.SetActive(true);
//        scopeOverlay.SetActive(false);
//        switching.animator.SetBool("Scoped", false);

//        if (previousFOV <= 0) yield break;
//        mainCamera.fieldOfView = previousFOV;
//        while (mainCamera.fieldOfView < previousFOV)
//        {
//            mainCamera.fieldOfView = mainCamera.fieldOfView + openSpeed;
//            yield return new WaitForSeconds(0.01f);
//        }
//    }

//    IEnumerator OnScoped()
//    {
//        isScoped = true;
//        switching.animator.SetBool("Scoped", true);
//        yield return new WaitForSeconds(0.23f);

//        if (!isScoped) yield break;
//        weaponCamera.SetActive(false);
//        scopeOverlay.SetActive(true);

//        previousFOV = mainCamera.fieldOfView;
//        mainCamera.fieldOfView = scopeFOV;
//        while (mainCamera.fieldOfView > scopeFOV)
//        {
//            mainCamera.fieldOfView = mainCamera.fieldOfView - openSpeed;
//            yield return new WaitForSeconds(0.01f);
//        }
//    }
//}