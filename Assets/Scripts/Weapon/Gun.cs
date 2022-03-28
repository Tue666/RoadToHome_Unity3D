using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public int damage = 20;
    public int ammoType = 0;
    public int currentAmmoCount = 0;
    public int maxAmmoCount = 40;
    public float range = 100;
    public float shootingRate = 0.07f;
    public float reloadTime = 2.6f;
    public string shootingClipName;
    public string reloadClipName;
    public ParticleSystem muzzleFlash = null;
    public TrailRenderer trail = null;
    public Transform barrel = null;
    // Scope
    public int scopeEquipped = 0;
    public int zoomSpeed = 1;
    // Recoil
    public float recoilX;
    public float recoilY;
    public float recoilReturnSpeed;

    public Animator animator;
}
