using UnityEngine;

[CreateAssetMenu(fileName = "BossSO", menuName = "Configuration/Boss")]
public class BossSO : ScriptableObject
{
    // Boss
    public Sprite avatar;
    public string bossName;
    public float speed;
    public float normalDamage;
    public float normalAttackRange;
    public float normalAttackRate;
    public float health;
    public float virtualBlood;
    public float healingBlood;
    public float defenseTime;
    public float dropItemsTime;
    public GameObject takeDamageEffect;
    public GameObject dropEffect;
    // Movement
    public float scanRange;
}
