using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _minSpeed = 0.5f;
    [SerializeField] private float _maxSpeed = 3.5f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float _detectRange = 10f;
    [SerializeField] private float attackRate = 3f;
    [SerializeField] private float wanderRange = 50f;
    [SerializeField] private AudioClip detectClip;

    private bool isDectecting = true;
    private bool isDetectClip = false;
    private float escapeRange = 1f;
    private float nearEnemiesChaseRange = 30f;
    private float mutableDetectRange;
    private float speed = 0.5f;
    private float nextAttackTime = 0f;
    private Animator animator;
    private AudioSource audioSource;
    private NavMeshAgent _agent;
    private Target target;
    private Player targetedPlayer;

    public float damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
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
    public Transform player
    {
        get { return _player; }
        set { _player = value; }
    }
    public NavMeshAgent agent
    {
        get { return _agent; }
        set { _agent = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = gameObject.GetComponent<Target>();
        if (_player == null) _player = GameObject.FindWithTag("Player").transform;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = detectClip;
        }
        targetedPlayer = _player.gameObject.GetComponent<Player>();
        animator = gameObject.GetComponent<Animator>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
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
        animator.SetFloat("Speed", minSpeed);
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
        float distance = Vector3.Distance(transform.position, _player.position);
        if ((!isDectecting && distance <= mutableDetectRange)) // Detected player
        {
            mutableDetectRange = _detectRange * 2.5f + escapeRange;
            if (!isDetectClip)
            {
                audioSource.Play();
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
            if (Vector3.Distance(transform.position, _player.position) <= _agent.stoppingDistance)
            {
                if (Time.time >= nextAttackTime)
                {
                    nextAttackTime = Time.time + attackRate;
                    AttackTargeted(_damage);
                }
            }
            animator.SetFloat("Speed", speed);
            _agent.speed = speed;
            _agent.SetDestination(_player.position);
        }
        else _agent.isStopped = true;
    }

    void AttackTargeted(float amount)
    {
        animator.SetTrigger("Attack");
        targetedPlayer.TakeDamage(amount);
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
