using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;

    private bool isRotating = false;
    private bool isChasing = false;
    private Boss bossCS;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        bossCS = gameObject.GetComponent<Boss>();
        agent.speed = bossCS.boss.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
            Rotate();
        if (isChasing)
            Chasing();
    }

    public void EnableRotate()
    {
        isRotating = true;
    }
    public void DisableRotate()
    {
        isRotating = false;
    }

    public bool IsChasing()
    {
        return isChasing;
    }
    public void EnableChasing()
    {
        agent.enabled = true;
        isChasing = true;
    }
    public void DisableChasing()
    {
        agent.enabled = false;
        isChasing = false;
    }

    public void EnableMovement()
    {
        isRotating = true;
        EnableChasing();
    }
    public void DisableMovement()
    {
        isRotating = false;
        DisableChasing();
    }

    void Rotate()
    {
        Vector3 dir = PlayerManager.Instance.playerObj.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
    }

    void Chasing()
    {
        float distance = Helpers.vector2DDistance(transform.position, PlayerManager.Instance.playerObj.transform.position);
        if (distance > bossCS.boss.scanRange) // Give up challenge
        {
            bossCS.shift = State.SLEEP;
            isRotating = false;
            isChasing = false;
            return;
        }
        if (agent.isOnNavMesh)
            agent.SetDestination(PlayerManager.Instance.playerObj.transform.position);
    }
}
