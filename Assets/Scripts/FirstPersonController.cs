using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip movementLoop;
    private AudioSource audioSource;
    private AudioSource movementAudioSource;

    [Header("Attack")]
    private float attackRange = 2f;
    public int attackDamage = 1;
    public KeyCode attackKey = KeyCode.Mouse0;
    public Animator swordAnimator;
    public bool isAttacking = false;

    [Header("Movement")]
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public float verticalLookLimit = 80f;

    [Header("Sprint")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Head Bob")]
    public float bobFrequency = 2f;
    public float bobAmplitude = 0.05f;

    private CharacterController controller;
    private float verticalVelocity;
    private float xRotation = 0f;
    private bool isSprinting;
    private float bobTimer = 0f;
    private Vector3 cameraDefaultLocalPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraDefaultLocalPos = cameraTransform.localPosition;

         audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        movementAudioSource = gameObject.AddComponent<AudioSource>();
        movementAudioSource.loop = true;
        movementAudioSource.playOnAwake = false;
        movementAudioSource.spatialBlend = 0f;


        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    void LateUpdate()
    {
        HandleMouseLook();
        HandleHeadBob();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (player body)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        bool isMoving = move.magnitude > 0.1f && controller.isGrounded;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Keeps player grounded
        }

        verticalVelocity += gravity * Time.deltaTime;

        isSprinting = Input.GetKey(sprintKey) && controller.isGrounded;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 velocity = move * currentSpeed;

        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
        HandeMovementSound(isMoving);
    }
    void HandleHeadBob()
    {
        if (!controller.isGrounded)
            return;

        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        if (horizontalVelocity.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            cameraTransform.localPosition = new Vector3(
                cameraDefaultLocalPos.x,
                cameraDefaultLocalPos.y + bobOffset,
                cameraDefaultLocalPos.z
            );
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                cameraDefaultLocalPos,
                Time.deltaTime * 5f
            );
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            isAttacking = true;
            PlaySwordAttack();
        }
    }

    public void PerformAttackHit()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange))
        {
            EnemyScript enemy = hit.collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Hit enemy! Health now: " + enemy.health);
            }
        }
    }

    public void OnAttackFinished()
    {
        Debug.Log("Finished");
        isAttacking = false;
    }

    void PlaySwordAttack()
    {
        PlayAttackSound();
        if (Random.value < 0.5f)
            swordAnimator.SetTrigger("Attack1");
        else
            swordAnimator.SetTrigger("Attack2");
    }

    void PlayAttackSound()
    {
        if (attackSounds == null || attackSounds.Length == 0)
            return;

        AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
        audioSource.PlayOneShot(clip);
    }

    void HandeMovementSound(bool isMoving)
    {
        if (!isMoving || movementLoop == null)
        {
            if (movementAudioSource.isPlaying)
                movementAudioSource.Stop();
            return;
        }

        if (!movementAudioSource.isPlaying)
        {
            movementAudioSource.clip = movementLoop;
            movementAudioSource.Play();
        }
    }
}
