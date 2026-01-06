using UnityEngine;

public class GameScript : MonoBehaviour
{
    private int towerHealth;

    public int TowerHealth
    {
        get { return towerHealth; }
        set { towerHealth = value; }
    }

    private float currentTime = 15f;
    private const float waveTime = 15f;

    private void SpawnWave()
    {
        
    }
    void Update()
    {
        float dt = Time.deltaTime;

        currentTime += dt;
        if (currentTime >= waveTime)
        {
            SpawnWave();
            currentTime = 0f;
        }
    }
}
