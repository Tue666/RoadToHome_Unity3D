using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private Camera cameraFOV = null;
    [SerializeField] private float attackRate = 0.1f;
    [SerializeField] private GameObject muzzleFlash = null;
    [SerializeField] private TrailRenderer trail = null;
    [SerializeField] private Transform barrel = null;
    [SerializeField] private AudioClip shotClip = null;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 2.6f;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private Recoil recoil;

    private AudioSource audioSource;
    private Animator animator;
    private float nextAttackTime = 0f;
    private int currentAmmo;
    private bool isReloading;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (recoil == null) gameObject.GetComponentInParent<Recoil>();
        if (cameraFOV == null) cameraFOV = GameObject.FindWithTag("Camera Recoil").GetComponent<Camera>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.priority = 0;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;
        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            Shooting();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (audioSource.isPlaying) yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.clip = reloadClip;
        audioSource.Play();
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shooting()
    {
        animator.SetTrigger("Attack");
        audioSource.clip = shotClip;
        audioSource.Play();
        currentAmmo--;
        recoil.RecoilAttacking();
        GameObject muzzle = Instantiate(muzzleFlash, barrel.position, barrel.rotation);
        Destroy(muzzle, 0.022f);
        TrailRenderer bullet = Instantiate(trail, barrel.position, barrel.rotation);
        bullet.AddPosition(barrel.position);
        RaycastHit hit;
        if (Physics.Raycast(cameraFOV.transform.position, cameraFOV.transform.forward, out hit))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage, hit.point);
            }
            SpawnTrail(bullet, hit.point);
        }
    }

    void SpawnTrail(TrailRenderer bullet, Vector3 hitPoint)
    {
        Vector3 startPosition = bullet.transform.position;
        float distance = Vector3.Distance(bullet.transform.position, hitPoint);
        float startDistance = distance;
        while (distance > 0)
        {
            bullet.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (distance / startDistance));
            distance -= Time.deltaTime * 100f;
        }
    }
}
