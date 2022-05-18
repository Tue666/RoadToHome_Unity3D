using UnityEngine;

[System.Serializable]
class PlayerData
{
    public float strength = 10f; //10f-99990f
    public int maxLevel = 30; //30-9999
    public int currentLevel = 1; //1-9999
    public float maxHealth = 200f; //200f-1999800f
    public float health = 200f; //200f-1999800f
    public float maxStamina = 200f; //200f
    public float stamina = 200f; //200f
    public float defense = 6f; //6f-59994f
    public float maxExp = 900f; //900f-3000300f
    public float currentExp = 0f; //0f-3000300f
    public float movementSpeed = 1f; //1f
    public float movementPlus = 0f; //0f-3f

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
    public float strength;
    public int maxLevel;
    public int currentLevel;
    public float maxHealth;
    public float health;
    public float maxStamina;
    public float stamina;
    public float defense;
    public float maxExp;
    public float currentExp;
    public float movementSpeed;
    public float movementPlus;

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
