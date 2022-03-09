using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 3.5f;
    [SerializeField] private float attackRange = 3f; // 1.8f

    private float speed = 0.5f;
    private float attackRate = 3f;
    private float nextAttackTime = 0f;
    private Animator animator;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        animator = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        StartCoroutine(RandomSpeed(4/speed));
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        //if (Vector3.Distance(transform.position, player.position) <= attackRange)
        //{
        //    if (Time.time >= nextAttackTime)
        //    {
        //        nextAttackTime = Time.time + attackRate;
        //        Attack(damage);
        //    }
        //    return;
        //}
        //transform.LookAt(player.position);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, 0, player.position.z), speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance)
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;
                Attack(damage);
            }
        }
        animator.SetFloat("Speed", speed);
        agent.speed = speed;
        agent.SetDestination(player.position);
    }

    void Attack(float amount)
    {
        animator.SetTrigger("Attack");
        Debug.Log("Hit player " + amount + " of health!");
    }

    IEnumerator RandomSpeed(float randomTime)
    {
        while (speed != 0)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
