using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxLevel = 30;
    [SerializeField] private float health = 200f;
    [SerializeField] private float defense = 5f;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthDrop;
    [SerializeField] private Text expBar;
    [SerializeField] private Text levelBar;

    private float maxHealth;
    private int currentExp = 0;
    private int maxExp;
    private int currentLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (playerController == null) playerController = gameObject.GetComponent<PlayerController>();
        UpdateStats();
        expBar.text = currentExp + "/" + maxExp;
        levelBar.text = "LV." + currentLevel;
    }

    void UpdateStats()
    {
        maxHealth = 200f * currentLevel; // 200 is health property
        maxExp = (currentLevel + 2) * 300;
        health = maxHealth; // Full of blood whenver level up
        defense += currentLevel * 2;
        switch (currentLevel)
        {
            case 5:
                playerController.movementPlus = 1f;
                break;
            case 15:
                playerController.movementPlus = 1.5f;
                break;
            case 30:
                playerController.movementPlus = 3f;
                break;
            default:
                break;
        }
    }

    public void IncreaseExp(int enemyLevel)
    {
        if (currentLevel < maxLevel)
        {
            currentExp += (65 - (enemyLevel * 2));
            int diff = maxExp - currentExp;
            if (diff <= 0)
            {
                currentLevel++;
                currentExp = Mathf.Abs(diff);
                UpdateStats();
                levelBar.text = "LV." + currentLevel;
            }
            expBar.text = currentExp + "/" + maxExp;
        }
    }

    public void TakeDamage(float amount)
    {
        float dameTaken = amount - defense;
        if (dameTaken <= 0)
        {
            
        }
        else
        {
            health -= (amount - defense);
            healthBar.fillAmount = health / maxHealth;
            float dropTime = 0f;
            StartCoroutine(UpdateHealthDrop(dropTime));
        }
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator UpdateHealthDrop(float dropTime)
    {
        while (healthBar.fillAmount < healthDrop.fillAmount)
        {
            healthDrop.fillAmount = Mathf.Lerp(healthDrop.fillAmount, healthBar.fillAmount, dropTime);
            dropTime += Time.deltaTime * 5f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Die()
    {
        Debug.Log("Game Over!");
    }
}
