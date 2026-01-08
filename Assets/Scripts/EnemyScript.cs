using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] private GameObject bloodParticlePrefab;
    [SerializeField] private AudioClip[] hitSounds;
    private AudioSource audioSource;

    public Animator animator;
    private GameScript gameScript;
    private const float speed = 2f;
    public int health = 2;
    private Vector3 targetPosition = Vector3.zero;
    private const float stopDistance = 5f;

    private const float damageTime = 3f;
    private float damageTimer = 0f;

    private float hitStunTime = 1.5f;
    private float hitStunTimer = 0f;

    

    void Start()
    {
        gameScript = FindAnyObjectByType<GameScript>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        health = Random.Range(1, 3);
    }
    void Update()
    {
        if(hitStunTimer > 0f)
        {
            hitStunTimer -= Time.deltaTime;
            animator.SetBool("IsMoving", false);
            return;
        }

        Vector3 flatPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatTarget = new Vector3(targetPosition.x, 0f, targetPosition.z);
        Vector3 direction = (flatTarget - flatPosition).normalized;

        float distance = Vector3.Distance(flatPosition, flatTarget);
        bool isMoving = distance > stopDistance;
        animator.SetBool("IsMoving", isMoving);
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
                animator.SetTrigger("Attack");
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        hitStunTimer = hitStunTime;
        animator.SetTrigger("Hit");
        PlayRandomHitSound();
        SpawnBlood();
        if (health <= 0)
        {
            Destroy(gameObject);
            PlaySoundAndDetach(GetRandomHitSound());
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

    void PlayRandomHitSound()
    {
        if (hitSounds == null || hitSounds.Length == 0 || audioSource == null)
            return;

        int index = Random.Range(0, hitSounds.Length);
        audioSource.PlayOneShot(hitSounds[index]);
    }

    void PlaySoundAndDetach(AudioClip clip)
    {
        if (clip == null) return;

        GameObject audioObject = new GameObject("TempAudio");
        audioObject.transform.position = transform.position;

        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 1f;
        source.Play();

        Destroy(audioObject, clip.length);
    }

    AudioClip GetRandomHitSound()
    {
        if (hitSounds == null || hitSounds.Length == 0)
            return null;

        return hitSounds[Random.Range(0, hitSounds.Length)];
    }
}
