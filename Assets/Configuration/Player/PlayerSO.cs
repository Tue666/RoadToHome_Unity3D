using UnityEngine;

[System.Serializable]
class PlayerData
{
    public float strength = 10f;
    public int maxLevel = 30;
    public int currentLevel = 1;
    public float maxHealth = 200f;
    public float health = 200f;
    public float maxStamina = 200f;
    public float stamina = 200f;
    public float defense = 6f;
    public float maxExp = 900f;
    public float currentExp = 0f;
    public float movementSpeed = 1f;
    public float movementPlus = 0f;

    public PlayerData() { }

    public PlayerData(PlayerSO player)
    {
        this.strength = player.strength;
        this.maxLevel = player.maxLevel;
        this.currentLevel = player.currentLevel;
        this.maxHealth = player.maxHealth;
        this.health = player.health;
        this.maxStamina = player.maxStamina;
        this.stamina = player.stamina;
        this.defense = player.defense;
        this.maxExp = player.maxExp;
        this.currentExp = player.currentExp;
        this.movementSpeed = player.movementSpeed;
        this.movementPlus = player.movementPlus;
    }
}

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Configuration/Player")]
public class PlayerSO : ScriptableObject
{
    public float strength = 10f;
    public int maxLevel = 30;
    public int currentLevel = 1;
    public float maxHealth = 200f;
    public float health = 200f;
    public float maxStamina = 200f;
    public float stamina = 200f;
    public float defense = 6f;
    public float maxExp = 900f;
    public float currentExp = 0f;
    public float movementSpeed = 1f;
    public float movementPlus = 0f;

    public void SavePlayer()
    {
        PlayerData data = new PlayerData(this);
        SaveSystem.Save("player", data);
    }

    public void LoadPlayer()
    {
        PlayerData player = new PlayerData();
        if (PlayerPrefs.GetString("GAME_MODE") == "continue")
        {
            player = SaveSystem.Load("player") as PlayerData;
        }
        this.strength = player.strength;
        this.maxLevel = player.maxLevel;
        this.currentLevel = player.currentLevel;
        this.maxHealth = player.maxHealth;
        this.health = player.health;
        this.maxStamina = player.maxStamina;
        this.stamina = player.stamina;
        this.defense = player.defense;
        this.maxExp = player.maxExp;
        this.currentExp = player.currentExp;
        this.movementSpeed = player.movementSpeed;
        this.movementPlus = player.movementPlus;
        //SceneLoader.Instance.LoadMap(PlayerPrefs.GetInt("Map"));
    }
}
