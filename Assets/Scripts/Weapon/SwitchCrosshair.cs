using System.Collections;
using UnityEngine;

public class SwitchCrosshair : MonoBehaviour
{
    [SerializeField] private Camera cameraFOV;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject focused;
    [SerializeField] private Animator animator;

    private bool currentFocus = false;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraFOV == null) cameraFOV = GameObject.FindWithTag("Camera Recoil").GetComponent<Camera>();
        if (crosshair == null)
        {
            foreach (Transform item in transform)
            {
                if (item.gameObject.tag == "Crosshair")
                {
                    crosshair = item.gameObject;
                    animator = item.GetComponent<Animator>();
                    break;
                }
            }
        }
        if (focused == null)
        {
            foreach (Transform item in transform)
            {
                if (item.gameObject.tag == "Focused")
                {
                    crosshair = item.gameObject;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SwitchOnFocusedTarget());
        StartCoroutine(SwitchOnScope());
    }

    IEnumerator SwitchOnFocusedTarget()
    {
        bool previousFocus = currentFocus;
        currentFocus = false;
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraFOV.transform.position, cameraFOV.transform.forward, out hit))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    currentFocus = true;
                }
            }
        }
        if (previousFocus != currentFocus) focused.SetActive(currentFocus);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator SwitchOnScope()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetBool("Aiming", true);
            yield return new WaitForSeconds(0.25f);
            crosshair.SetActive(false);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            crosshair.SetActive(true);
            animator.SetBool("Aiming", false);
        }
    }
}
