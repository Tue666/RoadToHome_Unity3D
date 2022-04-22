using System.Collections;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    [SerializeField] private EnemySO enemy;
    [SerializeField] private DropTableSO dropTable;

    private float _damage = 20f;
    private float health = 100f;
    private float defense = 5f;

    private bool _isDie = false;
    private bool _isChasing = false;
    private int currentExp = 0;
    private int maxExp;
    private int currentLevel = 1;
    private Animator _animator;
    private Movement movement;
    private AutoSpawner autoSpawner;
    private TMP_Text nameBar;

    public float damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
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
        nameBar = GetComponentInChildren<TMP_Text>();
        currentLevel = enemy.difficult; // temp
        UpdateStats();
        StartCoroutine(Evolution(enemy.evolutionSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        if (_isChasing)
        {
            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);
            movement.ChaseThePlayer(distance);
        }
    }

    void UpdateStats()
    {
        _damage *= currentLevel;
        health *= currentLevel;
        defense *= currentLevel;
        maxExp = (currentLevel + 2) * 300;
        movement.maxSpeed += (currentLevel - (currentLevel / 2f));
        movement.detectRange += currentLevel;
        float scale = (currentLevel * 0.1f) - 0.1f;
        transform.localScale = transform.localScale + new Vector3(scale, scale, scale);
        nameBar.text = Helpers.ReplaceInRegionOfSpecialCharacters(nameBar.text, '[', ']', "Lv." + currentLevel);
    }

    public void TakeDamage(float amount, Vector3 point)
    {
        if (!_isDie)
        {
            _isChasing = true;

            if (Helpers.HasParameter("Take Damage", _animator) && !Helpers.IsAnimatorPlayingAny(_animator))
                _animator.SetTrigger("Take Damage");

            float dameTaken = amount - defense;
            if (dameTaken <= 0)
            {
                GameObject defenseObject = Instantiate(enemy.defenseEffect, point, Quaternion.identity);
                Destroy(defenseObject, 1f);
            }
            else
            {
                GameObject bloodObject = Instantiate(enemy.takeDamageEffect, point, Random.rotation);
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
        PlayerManager.Instance.IncreaseExp(enemy.difficult);
        int currentIndex = autoSpawner.spawnIndex;
        autoSpawner.spawnIndex--;
        if (currentIndex == autoSpawner.spawnNumber) autoSpawner.ReSpawn();
        Drop();
    }

    void Drop()
    {
        if (dropTable == null) return;
        GameObject dropObject = Instantiate(enemy.dropEffect, transform.position, Quaternion.identity);
        dropObject.GetComponent<GenerateDropItems>().InitializeDropTable(dropTable);
        Destroy(dropObject, enemy.dropItemsTime);
    }

    IEnumerator Evolution(float evolutionSpeed)
    {
        while (currentLevel < enemy.maxLevel)
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
                GameObject evolutionObject = Instantiate(enemy.evolutionEffect, transform.position, Quaternion.AngleAxis(-90f, Vector3.right));
                yield return new WaitForSeconds(12f);
                _animator.SetBool("Evolution", false);
                movement.agent.isStopped = false;
                Destroy(evolutionObject);
            }
            yield return new WaitForSeconds(evolutionSpeed + Random.Range(0, 10));
        }
    }
}
