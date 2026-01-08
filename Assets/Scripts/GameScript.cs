using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameScript : MonoBehaviour
{
    public HealthBarUI healthBarUI;
    private int towerHealth = 100;
    private float currentTime = 30f;
    private const float waveTime = 25f;
    private float spawnRadius = 25f;
    private const int waveLimit = 3;
    private int currentWave = 0;

    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource musicSource;

    [SerializeField] private TextMeshProUGUI towerHealthText;
    [SerializeField] private GameObject[] enemyPrefabs;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        musicSource.Play();
    }
    
    public void DamageTower(int amount)
    {
        towerHealth -= amount;
        healthBarUI.UpdateHealth(towerHealth);
        if (towerHealth <= 0)
        {
            towerHealth = 0;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("los_screen");
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
