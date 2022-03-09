using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private Camera fpsCamera = null;
    [SerializeField] private float attackRate = 0.1f;
    [SerializeField] private GameObject muzzleFlash = null;
    [SerializeField] private Transform barrel = null;
    [SerializeField] private AudioClip shotClip = null;

    private AudioSource audioSource;
    private Animator animator;
    private float nextAttackTime = 0f;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.priority = 0;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            Shooting();
        }
    }

    void Shooting()
    {
        audioSource.clip = shotClip;
        audioSource.Play();
        animator.SetTrigger("Attack");
        GameObject muzzle = Instantiate(muzzleFlash, barrel.position, barrel.rotation);
        Destroy(muzzle, 0.022f);
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
