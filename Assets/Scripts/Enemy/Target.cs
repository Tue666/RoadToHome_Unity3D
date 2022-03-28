using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private float evolutionSpeed = 20f;
    [SerializeField] private float _summonRatePercent = 0f;
    [SerializeField] private float health = 50f;
    [SerializeField] private float defense = 5f;
    [SerializeField] private GameObject takeDamageEffect;
    [SerializeField] private GameObject defenseEffect;
    [SerializeField] private GameObject evolutionEffect;

    private bool _isDie = false;
    private bool _isChasing = false;
    private int currentExp = 0;
    private int maxExp;
    private int currentLevel = 1;
    private Animator _animator;
    private Movement movement;
    private AutoSpawner autoSpawner;
    private Text nameBar;

    public bool isDie
    {
        get { return _isDie; }
        set { _isDie = value; }
    }
    public bool isChasing
    {
        get { return _isChasing; }
        set { _isChasing = value; }
    }
    public float summonRatePercent
    {
        get { return _summonRatePercent; }
        set { _summonRatePercent = value; }
    }
    public Animator animator
    {
        get { return _animator; }
        set { _animator = value; }
    }

    void InitializeIfNecessary()
    {
        if (autoSpawner == null) autoSpawner = GameObject.FindWithTag("Spawn Points").GetComponent<AutoSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeIfNecessary();
        movement = GetComponent<Movement>();
        _animator = GetComponent<Animator>();
        nameBar = GetComponentInChildren<Text>();
        UpdateStats();
        StartCoroutine(Evolution(evolutionSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            float distance = Vector3.Distance(transform.position, movement.player.position);
            movement.ChaseThePlayer(distance);
        }
    }

    void UpdateStats()
    {
        maxExp = (currentLevel + 2) * 300;
        health += currentLevel * health;
        defense += currentLevel * 5;
        movement.damage += currentLevel * 5;
        movement.maxSpeed += (currentLevel - (currentLevel / 2f));
        movement.detectRange += currentLevel;
        float scale = (currentLevel * 0.2f) - 0.2f;
        transform.localScale = transform.localScale + new Vector3(scale, scale, scale);
        nameBar.text = Helpers.ReplaceInRegionOfSpecialCharacters(nameBar.text, '[', ']', "Lv." + currentLevel);
    }

    public void TakeDamage(float amount, Vector3 point)
    {
        if (!_isDie)
        {
            isChasing = true;
            if (Helpers.HasParameter("Take Damage", _animator) && !Helpers.IsAnimatorPlayingAny(_animator))
            {
                _animator.SetTrigger("Take Damage");
            }
            float dameTaken = amount - defense;
            if (dameTaken <= 0)
            {
                GameObject defenseObject = Instantiate(defenseEffect, point, Quaternion.identity);
                Destroy(defenseObject, 1f);
            }
            else
            {
                GameObject bloodObject = Instantiate(takeDamageEffect, point, Random.rotation);
                Destroy(bloodObject, 0.3f);
                health -= (amount - defense);
            }
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        _isDie = true;
        _animator.SetTrigger("Death");
        AudioManager.Instance.PlayEffect("ENEMY", "Enemy Death");
        PlayerManager.Instance.IncreaseExp(currentLevel);
        int currentIndex = autoSpawner.spawnIndex;
        autoSpawner.spawnIndex--;
        if (currentIndex == autoSpawner.spawnNumber) autoSpawner.ReSpawn();
    }

    IEnumerator Evolution(float evolutionSpeed)
    {
        while (currentLevel < maxLevel)
        {
            currentExp += (20 - currentLevel);
            int diff = maxExp - currentExp;
            if (diff <= 0)
            {
                currentLevel++;
                currentExp = Mathf.Abs(diff);
                UpdateStats();
                movement.agent.isStopped = true;
                _animator.SetBool("Evolution", true);
                Instantiate(evolutionEffect, transform.position, Quaternion.AngleAxis(-90f, Vector3.right));
                yield return new WaitForSeconds(12f);
                _animator.SetBool("Evolution", false);
                movement.agent.isStopped = false;
            }
            yield return new WaitForSeconds(evolutionSpeed + Random.Range(0, 10));
        }
    }
}
