using UnityEngine;

public class Gun : MonoBehaviour
{
    public ItemSO ammo;
    public string gunName;
    public Sprite gunIcon;
    public int damage = 35;
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

    #region Events
    public void PlayRunEffect()
    {
        AudioManager.Instance.PlayEffect("PLAYER", "Run");
    }
    #endregion
}
