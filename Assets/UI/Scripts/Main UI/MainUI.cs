using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public Image weaponIcon;
    public TMP_Text currentAmmo;
    public TMP_Text remainingAmmo;
}

public class MainUI : MonoBehaviour
{
    public static MainUI Instance { get; private set; }

    // Weapon Bar
    public Weapon[] weapons;
    // Health Bar
    public TMP_Text exp;
    public TMP_Text level;
    public Image healthBar;
    public Image healthDrop;
    public Image expBar;
    // Stamina Bar
    public Image staminaBar;
    // Take Damage Screen
    public GameObject bloodScreen;
    public GameObject directionIndicator;
    // Potions
    public Image potionCountdownIconOne;
    public Image potionCountdownIconTwo;

    private int currentWeaponIndex;

    private int potionCountdownTimeOne = 10;
    private int potionCountdownTimeTwo = 10;

    private WaitForSeconds waitCountdownPotion = new WaitForSeconds(1f);

    private WaitForSeconds waitShowBloodScreen = new WaitForSeconds(1f);
    private WaitForSeconds waitShowDirection = new WaitForSeconds(1f);
    private WaitForSeconds waitHealthDrop = new WaitForSeconds(0.4f);

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void HandleNoAmmoColor(int currentWeaponIndex)
    {
        weapons[currentWeaponIndex].currentAmmo.color = weapons[currentWeaponIndex].currentAmmo.text == "0" ? Color.red : Color.white;
        weapons[currentWeaponIndex].remainingAmmo.color = weapons[currentWeaponIndex].remainingAmmo.text == "0" ? Color.red : Color.white;
    }

    public void InitMainWeaponBar(Gun[] guns, Gun currentGun)
    {
        int index = 0;
        foreach (Gun gun in guns)
        {
            if (gun == currentGun)
                currentWeaponIndex = index;
            else
            {
                Image slotWeaponIcon = weapons[index].weaponIcon.rectTransform.parent.GetComponent<Image>();
                Color color = slotWeaponIcon.color;
                color.a = 0.7f;
                slotWeaponIcon.color = color;
                weapons[index].weaponIcon.color = color;
            }
            weapons[index].weaponName = gun.gunName;
            weapons[index].weaponIcon.sprite = gun.gunIcon;
            weapons[index].currentAmmo.text = gun.currentAmmoCount.ToString();
            weapons[index].remainingAmmo.text = InventoryManager.Instance.GetItem(gun.ammo).quantity.ToString();
            HandleNoAmmoColor(index);
            index++;
        }
    }

    public void CurrentWeaponChanged(string weaponName)
    {
        int index = 0;
        foreach (Weapon weapon in weapons)
        {
            if (string.IsNullOrEmpty(weapon.weaponName)) continue;
            Image slotWeaponIcon = weapon.weaponIcon.rectTransform.parent.GetComponent<Image>();
            Color color = slotWeaponIcon.color;
            if (weapon.weaponName.Equals(weaponName))
            {
                color.a = 1f;
                currentWeaponIndex = index;
            }
            else
            {
                color.a = 0.7f;
            }
            slotWeaponIcon.color = color;
            weapon.weaponIcon.color = color;
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

    public void ExpChanged(float currentExp, float maxExp)
    {
        exp.text = currentExp + " / " + maxExp;
    }

    public void LevelChanged(float currentLevel)
    {
        level.text = currentLevel.ToString();
    }

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

    public IEnumerator ShakeScreen(float duration, Transform cameraShake)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            cameraShake.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraShake.localPosition = Vector3.zero;
    }

    public IEnumerator ShowBloodScreen()
    {
        bloodScreen.SetActive(true);
        yield return waitShowBloodScreen;
        bloodScreen.SetActive(false);
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

    public void UpdateExpBar(float fillAmount)
    {
        expBar.fillAmount = fillAmount;
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
                    potionCDIcon = potionCountdownIconOne;
                    CDTime = potionCountdownTimeOne;
                }
                break;
            case 2:
                {
                    potionCDIcon = potionCountdownIconTwo;
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
}
