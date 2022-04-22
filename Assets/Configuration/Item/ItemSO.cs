using UnityEngine;

public enum Rank
{
    B,
    A,
    S,
    SS,
    SSS
}

public enum Type
{
    Ammo,
    Material,
    Gemstone,
    Potion,
    Story,
    Coin
}

public enum Function
{
    TRASH, // Will be default switch case and do nothing
    HEAL_HEALTH,
    HEAL_STAMINA
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Configuration/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Rank rank;
    public Type type;
    public Sprite icon;
    public Function function;
    [Multiline()]
    public string description;
    public int maximum;
    public int weight;
    public float sellPrice;
    public bool isUnique;

    public void PlayFunction()
    {
        if (InventoryManager.Instance.GetItem(this).quantity <= 0) return;

        switch (function)
        {
            case Function.TRASH:
                break;
            case Function.HEAL_HEALTH:
                {
                    float amount = PlayerManager.Instance.maxHealth * 0.2f;
                    if (PlayerManager.Instance.health + amount > PlayerManager.Instance.maxHealth)
                    {
                        PlayerManager.Instance.health = PlayerManager.Instance.maxHealth;
                    }
                    else
                        PlayerManager.Instance.health += amount;
                    MainUI.Instance.UpdateHealthBar(PlayerManager.Instance.health / PlayerManager.Instance.maxHealth);
                }
                break;
            case Function.HEAL_STAMINA:
                {
                    float amount = PlayerManager.Instance.maxStamina * 0.15f;
                    if (PlayerManager.Instance.stamina + amount > PlayerManager.Instance.maxStamina)
                    {
                        PlayerManager.Instance.stamina = PlayerManager.Instance.maxStamina;
                    }
                    else
                        PlayerManager.Instance.stamina += amount;
                    MainUI.Instance.UpdateStaminaBar(PlayerManager.Instance.stamina / PlayerManager.Instance.maxStamina);
                }
                break;
            default:
                break;
        }
    }
}
