using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;
    [SerializeField] private Gun[] gunsEquiqqed;

    private float nextShootingTime;
    private bool isReloading;
    private Camera cameraFOV = null;
    private RaycastHit hit;

    // Scope
    private int[] scopedFOV = { 10, 0, 0, 0, 0, 0, 0, 0, 20 }; // FOV from no scope to scope x8
    private bool isScoped = false;
    private float normalFOV;
    // Recoil
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private WaitForSeconds waitScope = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        cameraFOV = PlayerManager.Instance.cameraRecoil.GetComponent<Camera>();
        WeaponManager.currentWeapon = currentGun.gameObject.transform;
        WeaponManager.currentAnimator = currentGun.animator;
        normalFOV = cameraFOV.fieldOfView;
        MainUI.Instance.InitMainWeaponBar(gunsEquiqqed, currentGun);
    }

    // Update is called once per frame
    void Update()
    {
        if (WeaponManager.isActivating == "GUN")
        {
            ReturnRecoil();
            if (isReloading) return;
            if (Input.GetKeyDown(KeyCode.R) && currentGun.currentAmmoCount < currentGun.maxAmmoCount)
            {
                StartCoroutine(Reload());
                return;
            }

            // Must lock cursor to do the following
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                if (Input.GetButton("Fire1") && Time.time >= nextShootingTime)
                {
                    nextShootingTime = Time.time + currentGun.shootingRate;
                    if (currentGun.currentAmmoCount > 0)
                        Shooting();
                    else
                        AudioManager.Instance.PlayEffect("WEAPON", "Out Of Ammo");
                }
                if (Input.GetButtonDown("Fire2") && currentGun != null)
                {
                    StartCoroutine(OnScoped());
                }
                if (Input.GetButtonUp("Fire2") && currentGun != null)
                {
                    StartCoroutine(OnUnScoped());
                }
            }
        }
    }

    void ReturnRecoil()
    {
        if (targetRotation != Vector3.zero)
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, currentGun.recoilReturnSpeed * Time.deltaTime);
            currentRotation = Vector3.Lerp(currentRotation, targetRotation, 6f * Time.deltaTime);
            cameraFOV.transform.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.Instance.PlayEffect("WEAPON", currentGun.reloadClipName);
        currentGun.animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(currentGun.reloadTime);
        currentGun.animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        currentGun.currentAmmoCount = currentGun.maxAmmoCount;
        MainUI.Instance.CurrentAmmoChanged(currentGun.currentAmmoCount);
        isReloading = false;
    }

    void Shooting()
    {
        AudioManager.Instance.PlayEffect("WEAPON", currentGun.shootingClipName);
        currentGun.animator.SetTrigger("Attack");
        currentGun.muzzleFlash.Play();
        currentGun.currentAmmoCount--;
        MainUI.Instance.CurrentAmmoChanged(currentGun.currentAmmoCount);
        targetRotation += new Vector3(Random.Range(0, currentGun.recoilX), Random.Range(-currentGun.recoilY, currentGun.recoilY), 0);
        TrailRenderer bullet = Instantiate(currentGun.trail, currentGun.barrel.position, currentGun.barrel.rotation);
        bullet.AddPosition(currentGun.barrel.position);
        if (Physics.Raycast(cameraFOV.transform.position, cameraFOV.transform.forward, out hit, currentGun.range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(currentGun.damage, hit.point);
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

    IEnumerator OnUnScoped()
    {
        isScoped = false;
        currentGun.animator.SetBool("Scoped", false);
        while (cameraFOV.fieldOfView < normalFOV)
        {
            if (isScoped) yield break;
            float diff = cameraFOV.fieldOfView + currentGun.zoomSpeed;
            if (diff > normalFOV)
            {
                cameraFOV.fieldOfView = normalFOV;
                yield break;
            }
            cameraFOV.fieldOfView = diff;
            yield return waitScope;
        }
    }

    IEnumerator OnScoped()
    {
        isScoped = true;
        currentGun.animator.SetBool("Scoped", true);
        float newFOV = cameraFOV.fieldOfView - scopedFOV[currentGun.scopeEquipped];
        while (cameraFOV.fieldOfView > newFOV)
        {
            if (!isScoped) yield break;
            float diff = cameraFOV.fieldOfView - currentGun.zoomSpeed;
            if (diff < newFOV)
            {
                cameraFOV.fieldOfView = newFOV;
                yield break;
            }
            cameraFOV.fieldOfView = diff;
            yield return waitScope;
        }
    }

    public void GunChange(Gun gun)
    {
        if (currentGun == gun) return;
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        AudioManager.Instance.PlayEffect("WEAPON", "Weapon Change");
        WeaponManager.currentWeapon = gun.gameObject.transform;
        WeaponManager.currentAnimator = gun.animator;
        WeaponManager.isActivating = "GUN";
        currentGun = gun;
        currentGun.gameObject.SetActive(true);
        currentGun.animator.SetTrigger("Get");
        MainUI.Instance.CurrentWeaponChanged(currentGun.gunName);
    }
}
