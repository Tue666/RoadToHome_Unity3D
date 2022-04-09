using UnityEngine;

[CreateAssetMenu(fileName = "HandSO", menuName = "Configuration/Weapon/Hand")]
public class HandSO : ScriptableObject
{
    public string handName;
    public int damage = 20;
    public float range = 100;
    public float attackRate = 1f;
    public string attackClipName;

    public Animator animator;
}
