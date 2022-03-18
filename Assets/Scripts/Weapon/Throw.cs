using System.Collections;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] private float force = 20f;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject generatePosition;
    [SerializeField] private AudioClip throwClip;

    private Animator animator;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ThrowGranade());
        }
    }

    IEnumerator ThrowGranade()
    {
        animator.SetTrigger("Throw");
        audioSource.clip = throwClip;
        audioSource.Play();
        yield return new WaitForSeconds(1.15f);
        GameObject grenade = Instantiate(grenadePrefab, generatePosition.transform.position, generatePosition.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(generatePosition.transform.forward * force, ForceMode.VelocityChange);
    }
}
