using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public Image weaponBackground;
    public Image weaponIcon;
    public TMP_Text currentAmmo;
    public TMP_Text remainingAmmo;
}

[System.Serializable]
public class ExtraWeapon
{
    public string weaponName;
    public Image weaponBackground;
    public Image weaponIcon;
}

[System.Serializable]
public class Potion
{
    public string potionName;
    public Image potionIcon;
    public TMP_Text potionCount;
    public Image potionCountdown;
}

public class MainUI : MonoBehaviour
{
    public static MainUI Instance { get; private set; }

    // Weapon Bar
    public Weapon[] weapons;
    public ExtraWeapon extraWeapon;

    private int currentWeaponIndex;
    //------------------------------------------------------------
    // Health Bar
    public TMP_Text exp;
    public TMP_Text level;
    public Image healthBar;
    public Image healthDrop;
    public Image expBar;

    private WaitForSeconds waitHealthDrop = new WaitForSeconds(0.4f);
    //------------------------------------------------------------
    // Stamina Bar
    public Image staminaBar;
    //------------------------------------------------------------
    // Crosshair
    public Transform crosshair;
    public GameObject gunReticle;
    public GameObject gunFocused;

    private IEnumerator toggleGunReticleCoroutine;
    //------------------------------------------------------------
    // Take Damage Screen
    public GameObject bloodScreen;
    public GameObject directionIndicator;

    private WaitForSeconds waitShowBloodScreen = new WaitForSeconds(1f);
    private WaitForSeconds waitShowDirection = new WaitForSeconds(1f);
    //------------------------------------------------------------
    // Potions
    public Potion[] potionsUI;

    private int potionCountdownTimeOne = 10;
    private int potionCountdownTimeTwo = 10;
    private WaitForSeconds waitCountdownPotion = new WaitForSeconds(1f);

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #region Weapon
    public void SwitchCrosshair(string isActivating)
    {
        int activatingInt = isActivating == "HAND" ? 0 : 1;
        int index = 0;
        foreach (Transform obj in crosshair)
        {
            obj.gameObject.SetActive(index == activatingInt);
            index++;
        }
    }

    public void StartToggleGunReticleCoroutine(bool isScoped)
    {
        if (!isScoped && toggleGunReticleCoroutine != null)
        {
            StopCoroutine(toggleGunReticleCoroutine);
            gunReticle.SetActive(false);
            gunReticle.SetActive(true);
        }
        toggleGunReticleCoroutine = ToggleGunReticle(isScoped);
        StartCoroutine(toggleGunReticleCoroutine);
    }

    IEnumerator ToggleGunReticle(bool isScoped)
    {
        if (isScoped)
        {
            Animator animator = gunReticle.GetComponent<Animator>();
            animator.SetBool("Aiming", true);
            yield return new WaitForSeconds(0.25f);
            gunReticle.SetActive(false);
        }
        else
            gunReticle.SetActive(true);
    }

    public void ToggleGunFocused(bool isFocusing)
    {
        gunFocused.SetActive(isFocusing);
    }

    void HandleNoAmmoColor(int currentWeaponIndex)
    {
        weapons[currentWeaponIndex].currentAmmo.color = weapons[currentWeaponIndex].currentAmmo.text == "0" ? Color.red : Color.white;
        weapons[currentWeaponIndex].remainingAmmo.color = weapons[currentWeaponIndex].remainingAmmo.text == "0" ? Color.red : Color.white;
    }

    public void InitMainWeaponBar(Gun[] guns)
    {
        int index = 0;
        foreach (Gun gun in guns)
        {
            if (gun.transform == WeaponManager.currentWeapon)
                currentWeaponIndex = index;
            else
            {
                CanvasGroup canvasGroup = weapons[index].weaponBackground.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0.6f;
            }
            weapons[index].weaponName = gun.gunName;
            weapons[index].weaponIcon.sprite = gun.gunIcon;
            weapons[index].currentAmmo.text = gun.currentAmmoCount.ToString();
            if (InventoryManager.Instance.ItemExists(gun.ammo))
                weapons[index].remainingAmmo.text = InventoryManager.Instance.GetItem(gun.ammo).quantity.ToString();
            HandleNoAmmoColor(index);
            index++;
        }
    }

    public void InitExtraWeaponBar(Hand hand)
    {
        if (hand.transform != WeaponManager.currentWeapon)
        {
            CanvasGroup canvasGroup = extraWeapon.weaponBackground.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0.6f;
            extraWeapon.weaponName = hand.handName;
        }
    }

    public void CurrentWeaponChanged(string type = "", string weaponName = "")
    {
        CanvasGroup extraCanvasGroup = extraWeapon.weaponBackground.GetComponent<CanvasGroup>();
        float groupAlpha = 0.6f;

        // Handle extra weapon
        if (type == "HAND" && extraWeapon.weaponName.Equals(weaponName))
            groupAlpha = 1f;
        else
            groupAlpha = 0.6f;
        extraCanvasGroup.alpha = groupAlpha;

        // Handle main weapons
        int index = 0;
        foreach (Weapon weapon in weapons)
        {
            CanvasGroup weaponCanvasGroup = weapon.weaponBackground.GetComponent<CanvasGroup>();
            if (weapon.weaponName.Equals(weaponName))
            {
                groupAlpha = 1f;
                currentWeaponIndex = index;
            }
            else
                groupAlpha = 0.6f;
            weaponCanvasGroup.alpha = groupAlpha;
            index++;
        }
    }

    public void CurrentAmmoChanged(int newValue)
    {
        weapons[currentWeaponIndex].currentAmmo.text = newValue.ToString();
        HandleNoAmmoColor(currentWeaponIndex);
    }

    public void RemainingAmmoChanged(int newValue)
    {
        weapons[currentWeaponIndex].remainingAmmo.text = newValue.ToString();
        HandleNoAmmoColor(currentWeaponIndex);
    }
    #endregion

    #region Status
    public void InitPotions(ItemSO[] potions)
    {
        int index = 0;
        foreach (ItemSO item in potions)
        {
            potionsUI[index].potionName = item.itemName;
            potionsUI[index].potionIcon.sprite = item.icon;
            if (InventoryManager.Instance.ItemExists(item))
                potionsUI[index].potionCount.text = InventoryManager.Instance.GetItem(item).quantity.ToString();
            index++;
        }
    }

    public void PotionCountChanged(int index, int newValue)
    {
        potionsUI[index].potionCount.text = newValue.ToString();
    }

    public void UpdateExpBar(float fillAmount)
    {
        expBar.fillAmount = fillAmount;
    }

    public void ExpChanged(float currentExp, float maxExp)
    {
        exp.text = currentExp + " / " + maxExp;
    }

    public void LevelChanged(float currentLevel)
    {
        level.text = currentLevel.ToString();
    }

    public void UpdateHealthBar(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }

    public IEnumerator UpdateHealthDrop()
    {
        float dropTime = 0f;
        while (healthBar.fillAmount < healthDrop.fillAmount)
        {
            healthDrop.fillAmount = Mathf.Lerp(healthDrop.fillAmount, healthBar.fillAmount, dropTime);
            dropTime += Time.deltaTime * 5f;
            yield return waitHealthDrop;
        }
    }

    public void UpdateStaminaBar(float fillAmount)
    {
        staminaBar.fillAmount = fillAmount;
    }

    public IEnumerator UsePotion(System.Action<bool> isUsing, int potionSlot)
    {
        isUsing(true);
        Image potionCDIcon = null;
        int CDTime = 0;
        switch (potionSlot)
        {
            case 1:
                {
                    potionCDIcon = potionsUI[0].potionCountdown;
                    CDTime = potionCountdownTimeOne;
                }
                break;
            case 2:
                {
                    potionCDIcon = potionsUI[1].potionCountdown;
                    CDTime = potionCountdownTimeTwo;
                }
                break;
            default:
                break;
        }
        int remaining = CDTime;
        while (remaining >= 0)
        {
            potionCDIcon.fillAmount = Mathf.InverseLerp(0, CDTime, remaining);
            remaining--;
            yield return waitCountdownPotion;
        }
        isUsing(false);
    }
    #endregion

    #region Other
    public IEnumerator ShowDirectionIndicator(GameObject attacker, Transform cameraShake)
    {
        Vector3 playerDirection = cameraShake.forward;
        Vector3 enemyDirection = attacker.transform.position - cameraShake.position;
        int sign = Vector3.Cross(playerDirection, enemyDirection).z < 0 ? -1 : 1;
        float angle = Vector3.Angle(playerDirection, enemyDirection) * sign;
        directionIndicator.SetActive(true);
        directionIndicator.GetComponent<RectTransform>().rotation = Quaternion.AngleAxis(angle, Vector3.back);
        yield return waitShowDirection;
        directionIndicator.SetActive(false);
    }

    public IEnumerator ShakeScreen(float intensity, float duration, Transform cameraShake)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            cameraShake.localPosition = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraShake.localPosition = Vector3.zero;
    }

    public IEnumerator FlyAway(float intensity, float duration, Vector3 direction, Transform player)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            player.localPosition = Vector3.Lerp(player.localPosition + new Vector3(0f, 0.1f, 0f), player.localPosition + (direction * intensity), 0.1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator ShowBloodScreen()
    {
        bloodScreen.SetActive(true);
        yield return waitShowBloodScreen;
        bloodScreen.SetActive(false);
    }
    #endregion
}
