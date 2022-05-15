using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;

    private Camera cameraFOV = null;
    private bool isReloading;
    private bool isFocusing;
    private float nextShootingTime;
    private RaycastHit hit;
    private GameObject previousObject = null;
    private object hitObject = null;

    // Scope
    private int[] scopedFOV = { 10, 0, 0, 0, 0, 0, 0, 0, 20 }; // FOV from no scope to scope x8
    private bool isScoped = false;
    private float normalFOV;
    // Recoil
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private IEnumerator reloadCoroutine;

    private WaitForSeconds waitScope = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        cameraFOV = PlayerManager.Instance.cameraRecoil;
        normalFOV = cameraFOV.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (WeaponManager.isActivating == "GUN")
        {
            ReturnRecoil();
            if (isReloading) return;
            if (currentGun.currentAmmoCount <= 0 || (Input.GetKeyDown(KeyCode.R) && currentGun.currentAmmoCount < currentGun.maxAmmoCount))
            {
                // Must have ammo for reload
                if (InventoryManager.Instance.ItemExists(currentGun.ammo) && InventoryManager.Instance.GetItem(currentGun.ammo).quantity > 0)
                {
                    reloadCoroutine = Reload();
                    StartCoroutine(reloadCoroutine);
                    return;
                }
            }

            // Must lock cursor to do the following
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Shooting
                bool previousFocusing = isFocusing;
                isFocusing = false;
                if (Input.GetButton("Fire1") && Time.time >= nextShootingTime)
                {
                    nextShootingTime = Time.time + currentGun.shootingRate;
                    if (currentGun.currentAmmoCount > 0)
                        Shooting();
                    else
                        AudioManager.Instance.PlayEffect("WEAPON", "Out Of Ammo");
                }
                if (previousFocusing != isFocusing) MainUI.Instance.ToggleGunFocused(isFocusing);

                // Scope
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
        // Turn off the scope if it's happening
        if (isScoped) StartCoroutine(OnUnScoped());

        isReloading = true;
        AudioManager.Instance.PlayMusic(currentGun.reloadClipName);
        currentGun.animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(currentGun.reloadTime);
        currentGun.animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        int diff = currentGun.maxAmmoCount - currentGun.currentAmmoCount;
        int remainingAmmo = InventoryManager.Instance.GetItem(currentGun.ammo).quantity;
        if (diff >= remainingAmmo)
        {
            InventoryManager.Instance.EditItem(new InventoryItem(currentGun.ammo, 0));
            currentGun.currentAmmoCount += remainingAmmo;
        }
        else
        {
            InventoryManager.Instance.EditItem(new InventoryItem(currentGun.ammo, remainingAmmo - (currentGun.maxAmmoCount - currentGun.currentAmmoCount)));
            currentGun.currentAmmoCount = currentGun.maxAmmoCount;
        }
        MainUI.Instance.CurrentAmmoChanged(currentGun.currentAmmoCount);
        MainUI.Instance.RemainingAmmoChanged(InventoryManager.Instance.GetItem(currentGun.ammo).quantity);
        isReloading = false;
    }

    public void CancelReload()
    {
        if (isReloading)
        {
            StopCoroutine(reloadCoroutine);
            isReloading = false;
        }
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
            if (hitObject == null || hit.transform.gameObject != previousObject)
            {
                if (hit.transform.tag == "Enemy")
                    hitObject = hit.transform.GetComponent<Target>();
                else if (hit.transform.tag == "Boss")
                    hitObject = hit.transform.GetComponent<Boss>();
                else
                {
                    hitObject = null;
                    return;
                }
                previousObject = hit.transform.gameObject;
            }
            if (hitObject != null)
            {
                if (hitObject is Target)
                    ((Target)hitObject).TakeDamage(currentGun.damage + PlayerManager.Instance.player.strength, hit.point);
                if (hitObject is Boss)
                    ((Boss)hitObject).TakeDamage(currentGun.damage + PlayerManager.Instance.player.strength, hit.point);
                isFocusing = true;
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

    public IEnumerator OnUnScoped()
    {
        isScoped = false;
        currentGun.animator.SetBool("Scoped", isScoped);
        MainUI.Instance.StartToggleGunReticleCoroutine(isScoped);
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
        currentGun.animator.SetBool("Scoped", isScoped);
        MainUI.Instance.StartToggleGunReticleCoroutine(isScoped);
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
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        AudioManager.Instance.PlayEffect("WEAPON", "Weapon Change");
        WeaponManager.currentWeapon = gun.gameObject.transform;
        WeaponManager.currentAnimator = gun.animator;
        WeaponManager.isActivating = "GUN";
        currentGun = gun;
        currentGun.gameObject.SetActive(true);
        currentGun.animator.SetTrigger("Get");
        MainUI.Instance.CurrentWeaponChanged(WeaponManager.isActivating, currentGun.gunName);
    }
}
