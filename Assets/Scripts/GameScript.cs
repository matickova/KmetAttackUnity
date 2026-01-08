using UnityEngine;
using TMPro;

public class GameScript : MonoBehaviour
{
    private int towerHealth = 100;
    private float currentTime = 30f;
    private const float waveTime = 25f;
    private float spawnRadius = 25f;
    private const int waveLimit = 3;
    private int currentWave = 0;

    [SerializeField] private TextMeshProUGUI towerHealthText;
    [SerializeField] private GameObject[] enemyPrefabs;

    void Start()
    {
        
    }
    
    public void DamageTower(int amount)
    {
        towerHealth -= amount;
        if (towerHealth <= 0)
        {
            towerHealth = 0;
        }
    }

    private Vector3 RandomSpawnPoint(float radius)
    {
        radius += Random.Range(-4f, 4f);
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;

        return new Vector3(x, 0f, z);
    }
    private void SpawnWave()
    {
        currentWave += 1;
        if (enemyPrefabs != null)
        {
            for (int i = 0; i <= 10; i++)
            {
                Vector3 spawnPosition = RandomSpawnPoint(spawnRadius);
                Quaternion rot = Quaternion.LookRotation(-spawnPosition);

                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[randomIndex], spawnPosition, rot);
            }

        }
    }
    void Update()
    {
        towerHealthText.text = "Tower Health: " + towerHealth;

        currentTime += Time.deltaTime;
        if (currentTime >= waveTime && currentWave < waveLimit)
        {
            SpawnWave();
            currentTime = 0f;
        }
    }
}
