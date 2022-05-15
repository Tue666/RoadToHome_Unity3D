using UnityEngine;

public class BossSkill : MonoBehaviour
{
    private float nextNormalAttackTime = 0f;
    private float dameDealt = 0; // Damage will be dealt to the player
    private Boss bossCS;
    private BossMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        bossCS = gameObject.GetComponent<Boss>();
        movement = gameObject.GetComponent<BossMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.IsChasing())
        {
            NormalAttack();
        }
    }

    void NormalAttack()
    {
        if (Helpers.vector2DDistance(transform.position, PlayerManager.Instance.playerObj.transform.position) <= bossCS.boss.normalAttackRange)
        {
            if (Time.time >= nextNormalAttackTime)
            {
                nextNormalAttackTime = Time.time + bossCS.boss.normalAttackRate;
                dameDealt = bossCS.boss.normalDamage;
                bossCS.animator.SetTrigger("Normal Attack");
            }
        }
    }

    #region Events
    public void DealDamage(string effect = "")
    {
        if (Helpers.vector2DDistance(transform.position, PlayerManager.Instance.playerObj.transform.position) <= 1)
        {
            if (Time.time >= nextNormalAttackTime)
            {
                dameDealt = bossCS.boss.normalDamage;
                PlayerManager.Instance.TakeDamage(dameDealt, gameObject, effect);
            }
        }
    }
    public void PlayerTakeDamage(string effect = "")
    {
        PlayerManager.Instance.TakeDamage(dameDealt, gameObject, effect);
    }
    #endregion
}
