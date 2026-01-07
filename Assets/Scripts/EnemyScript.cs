using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] private GameObject bloodParticlePrefab;

    private GameScript gameScript;
    private const float speed = 2f;
    public int health = 3;
    private Vector3 targetPosition = Vector3.zero;
    private const float stopDistance = 8f;

    private const float damageTime = 3f;
    private float damageTimer = 0f;

    void Start()
    {
        gameScript = FindAnyObjectByType<GameScript>();
        health = Random.Range(1, 4);
    }
    void Update()
    {
        Vector3 flatPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatTarget = new Vector3(targetPosition.x, 0f, targetPosition.z);
        Vector3 direction = (flatTarget - flatPosition).normalized;

        float distance = Vector3.Distance(flatPosition, flatTarget);
        if (distance > stopDistance)
        {
            transform.position += direction * speed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
        else
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                damageTimer = 0;
                gameScript.DamageTower(1);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        SpawnBlood();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void SpawnBlood()
    {
        if (bloodParticlePrefab == null) return;
        Vector3 spawnPos = transform.position + Vector3.up * 1.2f;
        Vector3 toCamera = (Camera.main.transform.position - spawnPos).normalized;
        if (toCamera.sqrMagnitude < 0.01f)
            toCamera = Vector3.forward;

        Instantiate(
            bloodParticlePrefab,
            spawnPos,
            Quaternion.LookRotation(toCamera)
            );
    }
}
