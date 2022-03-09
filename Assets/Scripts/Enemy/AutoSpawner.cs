using System.Collections;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int spawnCount = 10;

    private int spawnIndex = 1;
    private float spawnTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn(spawnTime));
    }

    IEnumerator Spawn(float spawnTime)
    {
        while (spawnIndex <= spawnCount)
        {
            int randomEnemy = Random.Range(0, enemies.Length);
            int randomPosition = Random.Range(0, spawnPoints.Length);
            Instantiate(enemies[randomEnemy], spawnPoints[randomPosition].transform.position, Quaternion.identity);
            spawnIndex++;
            yield return new WaitForSeconds(spawnTime);

            if (spawnCount == 0)
            {
                Debug.Log("Won! Wait for the next stage...");
            }
        }
    }
}
