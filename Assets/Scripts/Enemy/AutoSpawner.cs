using System.Collections;
using System.Linq;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int _spawnNumber = 10;

    private int _spawnIndex = 1;
    private float spawnTime = 3f;
    private float totalSummonRate;

    public int spawnNumber
    {
        get { return _spawnNumber; }
        set { _spawnNumber = value; }
    }
    public int spawnIndex
    {
        get { return _spawnIndex; }
        set { _spawnIndex = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Area");
        totalSummonRate = enemies.Sum(x => x.GetComponent<Target>().summonRatePercent / 100);
        ReSpawn();
    }

    IEnumerator Spawn(float spawnTime)
    {
        while (_spawnIndex <= _spawnNumber)
        {
            float randomSummonRate = Random.Range(0, totalSummonRate);
            float currentRate;
            float nextRate = 0;
            float currentTotal = 0;
            foreach (GameObject enemy in enemies)
            {
                Target target = enemy.GetComponent<Target>();
                currentRate = nextRate;
                nextRate = target.summonRatePercent / 100;
                currentTotal += nextRate;
                if (randomSummonRate >= currentRate && (randomSummonRate < nextRate || randomSummonRate < currentTotal))
                {
                    int randomPosition = Random.Range(0, spawnPoints.Length);
                    Instantiate(enemy, spawnPoints[randomPosition].transform.position, Quaternion.identity);
                    _spawnIndex++;
                    break;
                }
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void ReSpawn()
    {
        StartCoroutine(Spawn(spawnTime));
    }
}
