using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Configuration/Player")]
public class PlayerSO : ScriptableObject
{
    public int maxLevel = 30;
    public float maxHealth = 200f;
    public float maxStamina = 200f;
    public float defense = 6f;
    public float maxExp = 900f;
}
