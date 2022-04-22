using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Configuration/Enemy")]
public class EnemySO : ScriptableObject
{
    // Enemy
    public int difficult;
    public int maxLevel;
    public float summonRatePercent;
    public float evolutionSpeed;
    public float attackRange;
    public float attackRate;
    public float dropItemsTime;
    public GameObject enemyPrefab;
    public GameObject takeDamageEffect;
    public GameObject defenseEffect;
    public GameObject evolutionEffect;
    public GameObject dropEffect;

    // Movement
    public float wanderRange;
    public float escapeRange;
    public float nearEnemiesChaseRange;
}
