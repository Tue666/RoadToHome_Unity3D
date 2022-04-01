using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private int ammoType = 0;
    [SerializeField] private Sprite icon;
    [SerializeField] private float damage = 10f;
    [SerializeField] private Camera cameraFOV = null;
    [SerializeField] private float attackRate = 0.1f;
    [SerializeField] private ParticleSystem muzzleFlash = null;
    [SerializeField] private TrailRenderer trail = null;
    [SerializeField] private Transform barrel = null;
    [SerializeField] private AudioClip shotClip = null;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 2.6f;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private SpaceBag spaceBag;

    private float nextAttackTime = 0f;
    private int currentAmmo = 0;
    private bool isReloading;
    private AudioSource audioSource;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        if (cameraFOV == null) cameraFOV = GameObject.FindWithTag("Camera Recoil").GetComponent<Camera>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (spaceBag == null)
        {
            Debug.Log("Call space bag");
            spaceBag = GameObject.FindWithTag("Space Bag").GetComponent<SpaceBag>();
        }
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;
        if (spaceBag.GetAmmo(ammoType) > 0 && (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }
        if (currentAmmo > 0 && Input.GetButton("Fire1") && Time.time >= nextAttackTime)
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
        int diff = maxAmmo - currentAmmo;
        int remainingAmmo = spaceBag.GetAmmo(ammoType);
        if (diff >= remainingAmmo)
        {
            spaceBag.SetAmmo(ammoType, remainingAmmo * -1);
            currentAmmo += remainingAmmo;
        }
        else
        {
            spaceBag.SetAmmo(ammoType, (maxAmmo - currentAmmo) * -1);
            currentAmmo = maxAmmo;
        }
        isReloading = false;
    }

    void Shooting()
    {
        animator.SetTrigger("Attack");
        audioSource.clip = shotClip;
        audioSource.Play();
        currentAmmo--;
        muzzleFlash.Play();
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
