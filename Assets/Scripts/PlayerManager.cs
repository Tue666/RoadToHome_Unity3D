using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private int maxLevel = 30;
    private int currentLevel = 1;
    private float maxHealth = 200f;
    private float health = 200f;
    private float maxStamina = 100f;
    [HideInInspector] public float stamina = 100f;
    private float defense = 6f;
    private int maxExp = 900;
    private int currentExp = 0;
    [HideInInspector] public float movementSpeed = 1f;
    [HideInInspector] public float movementPlus = 0f;

    // UI
    public Text expBar;
    public Text levelBar;
    public GameObject bloodScreen;
    public GameObject directionIndicator;
    public Image healthBar;
    public Image healthDrop;
    public Image staminaBar;

    private IEnumerator staminaDropCoroutine;

    private WaitForSeconds waitStaminaDrop = new WaitForSeconds(0.5f);
    private WaitForSeconds waitShowDirection = new WaitForSeconds(1f);
    private WaitForSeconds waitShowBloodScreen = new WaitForSeconds(1f);
    private WaitForSeconds waitHealthDrop = new WaitForSeconds(0.4f);

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        levelBar.text = "LV." + currentLevel;
        expBar.text = currentExp + " / " + maxExp;
    }

    public void StartStaminaDrop(float amount)
    {
        if (staminaDropCoroutine != null)
            StopCoroutine(staminaDropCoroutine);
        staminaDropCoroutine = StaminaDrop(amount);
        StartCoroutine(staminaDropCoroutine);
    }

    IEnumerator StaminaDrop(float amount)
    {
        while (true)
        {
            if (stamina + amount > 100f)
            {
                stamina = 100f;
                yield break;
            }
            if (stamina + amount < 0f)
            {
                stamina = 0f;
                yield break;
            }
            stamina += amount;
            staminaBar.fillAmount = stamina / maxStamina;
            yield return waitStaminaDrop;
        }
    }

    public void LevelUp()
    {
        maxHealth = 200f * currentLevel; // 200 is health property
        maxExp = (currentLevel + 2) * 300;
        health = maxHealth; // Full of blood whenver level up
        defense = currentLevel * 6;
        switch (currentLevel)
        {
            case 5:
                movementPlus = 1f;
                break;
            case 15:
                movementPlus = 1.5f;
                break;
            case 30:
                movementPlus = 3f;
                break;
            default:
                break;
        }
    }

    public void IncreaseExp(int enemyLevel)
    {
        if (currentLevel >= maxLevel) return;
        currentExp += (65 - (enemyLevel * 2));
        int diff = maxExp - currentExp;
        if (diff <= 0)
        {
            currentLevel++;
            currentExp = Mathf.Abs(diff);
            LevelUp();
            levelBar.text = "LV." + currentLevel;
        }
        expBar.text = currentExp + " / " + maxExp;
    }

    public void TakeDamage(float amount, GameObject attacker)
    {
        float dameTaken = amount - defense;
        if (dameTaken <= 0)
        {
            // Do something like create a shield here
            return;
        }
        health -= (amount - defense);
        healthBar.fillAmount = health / maxHealth;
        AudioManager.Instance.PlayEffect("PLAYER", "Player Take Damage");
        float dropTime = 0f;
        Transform cameraShake = GameObject.FindWithTag("Camera Recoil").transform;
        StartCoroutine(ShowDirectionIndicator(attacker, cameraShake));
        StartCoroutine(ShakeScreen(1f, cameraShake));
        StartCoroutine(ShowBloodScreen());
        StartCoroutine(UpdateHealthDrop(dropTime));
        if (health <= 0)
        {
            Debug.Log("Dead!");
        }
    }

    IEnumerator ShowDirectionIndicator(GameObject attacker, Transform cameraShake)
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

    IEnumerator ShakeScreen(float duration, Transform cameraShake)
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

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.SetActive(true);
        yield return waitShowBloodScreen;
        bloodScreen.SetActive(false);
    }

    IEnumerator UpdateHealthDrop(float dropTime)
    {
        while (healthBar.fillAmount < healthDrop.fillAmount)
        {
            healthDrop.fillAmount = Mathf.Lerp(healthDrop.fillAmount, healthBar.fillAmount, dropTime);
            dropTime += Time.deltaTime * 5f;
            yield return waitHealthDrop;
        }
    }
}
