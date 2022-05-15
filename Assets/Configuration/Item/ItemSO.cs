using System;
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
    public string id;
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
        if (!InventoryManager.Instance.ItemExists(this) || InventoryManager.Instance.GetItem(this).quantity <= 0) return;

        switch (function)
        {
            case Function.TRASH:
                break;
            case Function.HEAL_HEALTH:
                {
                    float amount = PlayerManager.Instance.player.maxHealth * 0.2f;
                    if (PlayerManager.Instance.player.health + amount > PlayerManager.Instance.player.maxHealth)
                    {
                        PlayerManager.Instance.player.health = PlayerManager.Instance.player.maxHealth;
                    }
                    else
                        PlayerManager.Instance.player.health += amount;
                    MainUI.Instance.UpdateHealthBar(PlayerManager.Instance.player.health / PlayerManager.Instance.player.maxHealth);
                }
                break;
            case Function.HEAL_STAMINA:
                {
                    float amount = PlayerManager.Instance.player.maxStamina * 0.15f;
                    if (PlayerManager.Instance.player.stamina + amount > PlayerManager.Instance.player.maxStamina)
                    {
                        PlayerManager.Instance.player.stamina = PlayerManager.Instance.player.maxStamina;
                    }
                    else
                        PlayerManager.Instance.player.stamina += amount;
                    MainUI.Instance.UpdateStaminaBar(PlayerManager.Instance.player.stamina / PlayerManager.Instance.player.maxStamina);
                }
                break;
            default:
                break;
        }
    }
}
