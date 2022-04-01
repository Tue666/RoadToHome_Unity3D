using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _minSpeed = 0.5f;
    [SerializeField] private float _maxSpeed = 3.5f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float _detectRange = 10f;
    [SerializeField] private float attackRate = 3f;
    [SerializeField] private float wanderRange = 50f;

    private bool isDectecting = true;
    private bool isDetectClip = false;
    private float escapeRange = 1f;
    private float nearEnemiesChaseRange = 50f;
    private float mutableDetectRange;
    private float speed = 0.5f;
    private float nextAttackTime = 0f;
    private NavMeshAgent _agent;
    private Target target;

    public float minSpeed
    {
        get { return _minSpeed; }
        set { _minSpeed = value; }
    }
    public float maxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = value; }
    }
    public float detectRange
    {
        get { return _detectRange; }
        set { _detectRange = value; }
    }
    public NavMeshAgent agent
    {
        get { return _agent; }
        set { _agent = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Target>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = attackRange;
        mutableDetectRange = _detectRange;
        StartCoroutine(RandomSpeed(4/speed));
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
    }

    IEnumerator Wander()
    {
        mutableDetectRange = _detectRange;
        target.animator.SetFloat("Speed", minSpeed);
        _agent.speed = minSpeed;
        isDetectClip = false;
        target.isChasing = false;
        while (true)
        {
            Vector3 randomPosition = Random.insideUnitSphere * Random.Range(-wanderRange, wanderRange);
            randomPosition += transform.position;
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, wanderRange, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);
        if ((!isDectecting && distance <= mutableDetectRange)) // Detected player
        {
            mutableDetectRange = _detectRange * 2.5f + escapeRange;
            if (!isDetectClip)
            {
                AudioManager.Instance.PlayEffect("ENEMY", "Enemy Detect");
                isDetectClip = true;
                NearEnemiesChaseThePlayer();
            }
            ChaseThePlayer(distance);
        }
        if (isDectecting) // Wander for detect
        {
            StartCoroutine(Wander());
            isDectecting = false;
        }
    }

    void NearEnemiesChaseThePlayer()
    {
        Collider[] nearEnemies = Physics.OverlapSphere(transform.position, nearEnemiesChaseRange);
        foreach (Collider enemy in nearEnemies)
        {
            GameObject enemyObject = enemy.gameObject;
            if (enemyObject.tag == "Enemy" && enemyObject != transform.gameObject)
            {
                Target target = enemyObject.GetComponent<Target>();
                target.isChasing = true;
            }
        }
    }

    public void ChaseThePlayer(float distance)
    {
        if (target.isChasing) mutableDetectRange = _detectRange * 2 + escapeRange;
        if (mutableDetectRange - distance < escapeRange)
        {
            isDectecting = true;
            return;
        }
        if (!target.isDie)
        {
            if (Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) <= _agent.stoppingDistance)
            {
                if (Time.time >= nextAttackTime)
                {
                    nextAttackTime = Time.time + attackRate;
                    target.animator.SetTrigger("Attack");
                    PlayerManager.Instance.TakeDamage(target.damage, gameObject);
                }
            }
            target.animator.SetFloat("Speed", speed);
            _agent.speed = speed;
            _agent.SetDestination(PlayerManager.Instance.player.transform.position);
        }
        else _agent.isStopped = true;
    }

    IEnumerator RandomSpeed(float randomTime)
    {
        while (speed != 0)
        {
            speed = Random.Range(_minSpeed, _maxSpeed);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
