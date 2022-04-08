using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public GameObject player = null;
    public Camera cameraLook = null;
    public Camera cameraRecoil = null;

    private int maxLevel = 30;
    private int currentLevel = 1;
    private float maxHealth = 200f;
    private float health = 200f;
    private float maxStamina = 100f;
    [HideInInspector] public float stamina = 100f;
    private float defense = 6f;
    private float maxExp = 900f;
    private float currentExp = 0f;
    [HideInInspector] public float movementSpeed = 1f;
    [HideInInspector] public float movementPlus = 0f;

    private IEnumerator staminaDropCoroutine;

    void InitializeIfNecessary()
    {
        if (player == null) player = GameObject.FindWithTag("Player");
        if (cameraLook == null) cameraLook = GameObject.FindWithTag("Camera Look").GetComponent<Camera>();
        if (cameraRecoil == null) cameraRecoil = GameObject.FindWithTag("Camera Recoil").GetComponent<Camera>();
    }

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
        InitializeIfNecessary();
        MainUI.Instance.ExpChanged(currentExp, maxExp);
        MainUI.Instance.UpdateExpBar(currentExp / maxExp);
        MainUI.Instance.LevelChanged(currentLevel);
    }

    public void StartStaminaDrop(float amount, WaitForSeconds waitStaminaDrop)
    {
        if (staminaDropCoroutine != null)
            StopCoroutine(staminaDropCoroutine);
        staminaDropCoroutine = StaminaDrop(amount, waitStaminaDrop);
        StartCoroutine(staminaDropCoroutine);
    }

    IEnumerator StaminaDrop(float amount, WaitForSeconds waitStaminaDrop)
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
            MainUI.Instance.UpdateStaminaBar(stamina / maxStamina);
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
        float diff = maxExp - currentExp;
        if (diff <= 0)
        {
            currentLevel++;
            currentExp = Mathf.Abs(diff);
            LevelUp();
            MainUI.Instance.LevelChanged(currentLevel);
        }
        MainUI.Instance.ExpChanged(currentExp, maxExp);
        MainUI.Instance.UpdateExpBar(currentExp / maxExp);
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
        MainUI.Instance.UpdateHealthBar(health / maxHealth);
        AudioManager.Instance.PlayEffect("PLAYER", "Player Take Damage");
        Transform cameraShake = cameraRecoil.transform;
        StartCoroutine(MainUI.Instance.ShowDirectionIndicator(attacker, cameraShake));
        StartCoroutine(MainUI.Instance.ShakeScreen(1f, cameraShake));
        StartCoroutine(MainUI.Instance.ShowBloodScreen());
        StartCoroutine(MainUI.Instance.UpdateHealthDrop());
        if (health <= 0)
        {
            Debug.Log("Dead!");
        }
    }
}
