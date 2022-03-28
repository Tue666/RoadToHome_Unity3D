using System.Collections;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int _spawnNumber = 10;

    private int _spawnIndex = 1;
    private float spawnTime = 5f;
    private float[] summonRates;
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

    void InitializeIfNecessary()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Area");
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeIfNecessary();
        int i = 0;
        summonRates = new float[enemies.Length];
        foreach (GameObject enemy in enemies)
        {
            Target target = enemy.GetComponent<Target>();
            float rate = target.summonRatePercent / 100;
            summonRates[i] = rate;
            totalSummonRate += rate;
            i++;
        }
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
            int i = 0;
            foreach (GameObject enemy in enemies)
            {
                currentRate = nextRate;
                nextRate = summonRates[i];
                currentTotal += nextRate;
                if (randomSummonRate >= currentRate && (randomSummonRate < nextRate || randomSummonRate < currentTotal))
                {
                    int randomPosition = Random.Range(0, spawnPoints.Length);
                    Instantiate(enemy, spawnPoints[randomPosition].transform.position, Quaternion.identity);
                    _spawnIndex++;
                    i++;
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
