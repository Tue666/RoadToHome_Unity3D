using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public PlayerSO player;
    public GameObject playerObj = null;
    public Camera cameraLook = null;
    public Camera cameraRecoil = null;

    private IEnumerator staminaDropCoroutine;

    #region Weapon Equipped
    public Gun[] gunsEquiqqed;
    public Hand handEquipped;
    public ItemSO[] potionsEquipped;
    #endregion

    void InitializeIfNecessary()
    {
        if (playerObj == null) playerObj = gameObject;
        if (cameraLook == null) cameraLook = GameObject.FindWithTag("Camera Look").GetComponent<Camera>();
        if (cameraRecoil == null) cameraRecoil = GameObject.FindWithTag("Camera Recoil").GetComponent<Camera>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        player.LoadPlayer();
        InventoryManager.Instance.LoadInventory();

        InitializeIfNecessary();
        MainUI.Instance.ExpChanged(player.currentExp, player.maxExp);
        MainUI.Instance.UpdateExpBar(player.currentExp / player.maxExp);
        MainUI.Instance.LevelChanged(player.currentLevel);

        MainUI.Instance.InitMainWeaponBar(gunsEquiqqed);
        MainUI.Instance.InitExtraWeaponBar(handEquipped);
        MainUI.Instance.InitPotions(potionsEquipped);
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
            if (player.stamina + amount > player.maxStamina)
            {
                player.stamina = player.maxStamina;
                yield break;
            }
            if (player.stamina + amount < 0f)
            {
                player.stamina = 0f;
                yield break;
            }
            player.stamina += amount;
            MainUI.Instance.UpdateStaminaBar(player.stamina / player.maxStamina);
            yield return waitStaminaDrop;
        }
    }

    public void LevelUp()
    {
        SystemWindowManager.Instance.ShowWindowSystem("DEFAULT", "Level Up\nCongratulations!");
        player.strength = 10 * player.currentLevel;
        player.maxHealth = 200 * player.currentLevel;
        player.defense = 6 * player.currentLevel;
        player.maxExp = (player.currentLevel + 2) * 300;
        player.health = player.maxHealth; // Full of blood whenver level up
        MainUI.Instance.UpdateHealthBar(player.health / player.maxHealth);
        
        switch (player.currentLevel)
        {
            case 5:
                player.movementPlus = 1f;
                break;
            case 15:
                player.movementPlus = 1.5f;
                break;
            case 30:
                player.movementPlus = 3f;
                break;
            default:
                break;
        }
    }

    public void IncreaseExp(int difficult)
    {
        if (player.currentLevel >= player.maxLevel) return;
        player.currentExp += ((difficult * 170) - (player.currentLevel * 10));
        float diff = player.maxExp - player.currentExp;
        if (diff <= 0)
        {
            player.currentLevel++;
            player.currentExp = Mathf.Abs(diff);
            LevelUp();
            MainUI.Instance.LevelChanged(player.currentLevel);
        }
        MainUI.Instance.ExpChanged(player.currentExp, player.maxExp);
        MainUI.Instance.UpdateExpBar(player.currentExp / player.maxExp);
    }

    public void TakeDamage(float amount, GameObject attacker, string effect = "")
    {
        if (player.health <= 0) return;

        float dameTaken = amount - player.defense;
        if (dameTaken <= 0)
        {
            // Do something like create a shield here
            return;
        }
        player.health -= (amount - player.defense);
        MainUI.Instance.UpdateHealthBar(player.health / player.maxHealth);
        AudioManager.Instance.PlayEffect("PLAYER", "Player Take Damage");
        Transform cameraShake = cameraRecoil.transform;
        StartCoroutine(MainUI.Instance.ShowDirectionIndicator(attacker, cameraShake));
        StartCoroutine(MainUI.Instance.ShakeScreen(1f, 1f, cameraShake));
        StartCoroutine(MainUI.Instance.ShowBloodScreen());
        StartCoroutine(MainUI.Instance.UpdateHealthDrop());
        HandleEffect(effect, attacker.transform.forward);
        if (player.health <= 0)
        {
            if (Boss.beingChallenged)
            {
                Boss.beingChallenged = false;
            }
            UIManager.Instance.ShowView("GameOver Menu");
        }
    }

    public void HandleEffect(string effect, Vector3 direction = new Vector3())
    {
        switch (effect)
        {
            case "Shake":
                StartCoroutine(MainUI.Instance.ShakeScreen(0.3f, 0.2f, PlayerManager.Instance.cameraRecoil.transform));
                break;
            case "Fly Away":
                StartCoroutine(MainUI.Instance.ShakeScreen(0.3f, 0.2f, PlayerManager.Instance.cameraRecoil.transform));
                StartCoroutine(MainUI.Instance.FlyAway(5f, 0.4f, direction, PlayerManager.Instance.playerObj.transform));
                break;
            default:
                break;
        }
    }
}
