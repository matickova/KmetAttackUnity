using UnityEngine;
using TMPro;

public class GameScript : MonoBehaviour
{
    private int towerHealth = 100;
    private float currentTime = 30f;
    private const float waveTime = 30f;
    private float spawnRadius = 25f;

    [SerializeField] private TextMeshProUGUI towerHealthText;
    [SerializeField] private GameObject enemyPrefab;

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
        if (enemyPrefab != null)
        {
            for (int i = 0; i <= 10; i++)
            {
                Vector3 spawnPosition = RandomSpawnPoint(spawnRadius);
                Quaternion rot = Quaternion.LookRotation(-spawnPosition);
                Instantiate(enemyPrefab, spawnPosition, rot);
            }

        }
    }
    void Update()
    {
        towerHealthText.text = "Tower Health: " + towerHealth;

        currentTime += Time.deltaTime;
        if (currentTime >= waveTime)
        {
            SpawnWave();
            currentTime = 0f;
        }
    }
}
