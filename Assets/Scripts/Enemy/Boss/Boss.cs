using System.Collections;
using UnityEngine;

public enum State
{
    DEFAULT, // will trigger case default switch and do nothing
    SCAN,
    SLEEP,
    WAKE_UP,
    DEFENSE,
    DEATH
}

public class Boss : MonoBehaviour
{
    public static bool beingChallenged = false;

    public BossSO boss;
    public DropTableSO dropTable;
    public Transform startPosition;
    [HideInInspector]
    public State shift;
    [HideInInspector]
    public Animator animator;

    private float currentHealth;
    private float currentVirtualBlood;
    private bool isVirtualBlood = false;
    private bool isInviolable = true;
    private bool isDefenseMode = false;
    private bool reset = false;
    private BossMovement movement;

    private IEnumerator defenseCoroutine;
    private IEnumerator healingCoroutine;
    private WaitForSeconds waitDefense;

    // Start is called before the first frame update
    void Start()
    {
        shift = State.SCAN;
        currentHealth = boss.health;
        waitDefense = new WaitForSeconds(boss.defenseTime);
        movement = gameObject.GetComponent<BossMovement>();
        animator = gameObject.GetComponent<Animator>();
        BossUI.Instance.Initialize(boss.avatar, boss.bossName);
    }

    // Update is called once per frame
    void Update()
    {
        switch (shift)
        {
            case State.SCAN:
                Scan(Helpers.vector2DDistance(transform.position, startPosition.position));
                break;
            case State.SLEEP:
                Sleep();
                break;
            case State.WAKE_UP:
                WakeUp();
                break;
            case State.DEFENSE:
                StartDefenseCoroutine();
                break;
            case State.DEATH:
                Death();
                break;
            default:
                break;
        }
    }

    #region State
    public void ResetBoss()
    {
        reset = true;
        beingChallenged = false;
        isInviolable = false;
        currentHealth = boss.health;
        animator.SetTrigger("Reset");
        animator.SetBool("Walking", false);
        SystemWindowManager.Instance.ShowWindowSystem("DANGER", "Challenging the boss failed, CHICKENNNNNNN :)))");
        BossUI.Instance.HideHealthBar();
        AudioManager.Instance.StopBackground();
        BossUI.Instance.UpdateHealthBar(currentHealth / boss.health);
    }

    void Scan(float startPositionDistance)
    {
        float distance = Helpers.vector2DDistance(transform.position, PlayerManager.Instance.player.transform.position);
        if (distance <= boss.scanRange)
        {
            // The boss is currently in the sleeping position
            if (startPositionDistance == 0)
            {
                shift = State.WAKE_UP;
                movement.DisableChasing();
            }
            else // The boss is on it way to sleeping position
                Chasing();
        }
    }

    void Sleep()
    {
        float distance = Helpers.vector2DDistance(transform.position, startPosition.position);
        if (distance != 0)
            movement.agent.SetDestination(startPosition.position);
        else if (!reset)
            ResetBoss();
        Scan(distance);
    }

    void WakeUp()
    {
        shift = State.DEFAULT;
        reset = false;
        beingChallenged = true;
        movement.EnableRotate();
        animator.SetTrigger("Wake Up");
        SystemWindowManager.Instance.ShowWindowSystem("DANGER", "Start challenging the boss, good luck!");
        BossUI.Instance.ShowHealthBar();
        AudioManager.Instance.PlayBackground("Map 1 Challenge");
    }

    void StartDefenseCoroutine()
    {
        defenseCoroutine = Defense();
        StartCoroutine(defenseCoroutine);
    }
    IEnumerator Defense()
    {
        shift = State.DEFAULT;
        animator.SetBool("Defensing", true);
        animator.SetBool("Broken Armor", false);
        EnterDefenseMode();
        StartHealing();
        // Swap virtual blood
        currentVirtualBlood = boss.virtualBlood;
        BossUI.Instance.ShowVirtualBlood();
        isVirtualBlood = true;

        yield return waitDefense;

        IsBrokenArmor(false);
    }

    void Death()
    {
        shift = State.DEFAULT;
        isInviolable = true;
        animator.SetTrigger("Death");
        movement.DisableChasing();
    }
    #endregion

    #region Events
    // Start chasing imediately after Wake Up
    public void Chasing()
    {
        isInviolable = false;
        movement.EnableChasing();
        animator.SetBool("Walking", true);
    }
    public void Growl()
    {
        AudioManager.Instance.PlayEffect("ENEMY", "Boss Growl");
    }
    public void Walk(string effect = "")
    {
        AudioManager.Instance.PlayEffect("ENEMY", "Boss Walk");
        HandleEffectTakeDamage(effect);
    }
    public void Drop()
    {
        Victory();
        if (dropTable == null) return;
        GameObject dropObject = Instantiate(boss.dropEffect, transform.position, Quaternion.identity);
        dropObject.GetComponent<GenerateDropItems>().InitializeDropTable(dropTable);
        Destroy(dropObject, boss.dropItemsTime);
    }
    #endregion

    void StartHealing()
    {
        healingCoroutine = Healing();
        StartCoroutine(healingCoroutine);
    }
    IEnumerator Healing()
    {
        float elapsedTime = boss.defenseTime;
        while (elapsedTime > 0)
        {
            currentHealth += boss.healingBlood;
            if (currentHealth > boss.health)
            {
                currentHealth = boss.health;
                yield break;
            }
            BossUI.Instance.UpdateHealthBar(currentHealth / boss.health);
            elapsedTime--;
            yield return new WaitForSeconds(1f);
        }
    }

    public void EnterDefenseMode()
    {
        movement.DisableChasing();
        isDefenseMode = true;
    }
    public void ExitDefenseMode()
    {
        movement.EnableChasing();
        isDefenseMode = false;
    }
    void IsBrokenArmor(bool isBrokenArmor)
    {
        BossUI.Instance.HideVirtualBlood();
        isVirtualBlood = false;
        animator.SetBool("Defensing", false);
        if (isBrokenArmor)
        {
            if (healingCoroutine != null)
                StopCoroutine(healingCoroutine);
            animator.SetBool("Broken Armor", true);
            StopCoroutine(defenseCoroutine);
        }
    }

    public void TakeDamage(float amount, Vector3 point)
    {
        if (isInviolable) return;
        if (!isVirtualBlood && !isDefenseMode)
        {
            float randomTriggerDefense = Random.Range(0f, 1f);
            if (randomTriggerDefense < 0.015f)
                shift = State.DEFENSE;
        }

        GameObject takeDamageObject = Instantiate(boss.takeDamageEffect, point, Random.rotation);
        Destroy(takeDamageObject, 0.3f);

        if (isVirtualBlood)
        {
            currentVirtualBlood -= amount;
            BossUI.Instance.UpdateVirtualBlood(currentVirtualBlood / boss.virtualBlood);
        }
        else
        {
            currentHealth -= amount;
            BossUI.Instance.UpdateHealthBar(currentHealth / boss.health);
        }

        if (isVirtualBlood && currentVirtualBlood <= 0)
            IsBrokenArmor(true);
        if (currentHealth <= 0)
            shift = State.DEATH;
    }

    void Victory()
    {
        beingChallenged = false;
        SystemWindowManager.Instance.ShowWindowSystem("SUCCESS", "Successful boss challenge, PROOOOOOO :)))");
        BossUI.Instance.HideHealthBar();
        AudioManager.Instance.StopBackground();
    }

    public void HandleEffectTakeDamage(string effect)
    {
        switch (effect)
        {
            case "Shake":
                StartCoroutine(MainUI.Instance.ShakeScreen(0.3f, 0.2f, PlayerManager.Instance.cameraRecoil.transform));
                break;
            case "Fly Away":
                StartCoroutine(MainUI.Instance.ShakeScreen(0.3f, 0.2f, PlayerManager.Instance.cameraRecoil.transform));
                StartCoroutine(MainUI.Instance.FlyAway(5f, 0.4f, transform.forward, PlayerManager.Instance.player.transform));
                break;
            default:
                break;
        }
    }
}
