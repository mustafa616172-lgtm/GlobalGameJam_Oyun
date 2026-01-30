using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 0.5f;
    
    [Header("Ground Check")]
    [SerializeField] private float rayDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer = -1;
    
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    
    // Animator parameter names
    private readonly string SPEED_PARAMETER = "Speed";
    private readonly string IS_JUMPING = "IsJumping";
    private readonly string IS_GROUNDED = "IsGrounded";
    
    private bool isGrounded = true;
    private float currentSpeed = 0f;
    private bool isMoving = false;
    private bool isRunning = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        if (animator == null)
        {
            Debug.LogError("❌ Animator component not found on " + gameObject.name);
        }
        
        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody component not found on " + gameObject.name);
        }
        
        // Rigidbody konfigürasyonu
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Debug.Log("✓ Rigidbody configured for " + gameObject.name);
        }
    }

    private void Update()
    {
        HandleInput();
        CheckGrounded();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            // Gravity uygulanmaya devam etsin
            rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);
            
            // Drag ayarla (hava vs yer)
            rb.linearDamping = isGrounded ? groundDrag : airDrag;
        }
    }

    private void HandleInput()
    {
        // Reset movement
        moveDirection = Vector3.zero;
        currentSpeed = 0f;
        isMoving = false;
        isRunning = false;

        // W tuşu - Yürüme / Koşma
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = transform.forward;
            isMoving = true;
            
            // W + Left Shift - Koşma
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
                isRunning = true;
            }
            else
            {
                // Sadece W - Yürüme
                currentSpeed = walkSpeed;
                isRunning = false;
            }
        }
        
        // Space - Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            // Speed parameter'ı güncelle (Idle: 0, Walk: 0.5, Run: 1.0)
            float speedParameter = 0f;
            
            if (isMoving)
            {
                if (isRunning)
                {
                    speedParameter = 1.0f; // Koşma
                }
                else
                {
                    speedParameter = 0.5f; // Yürüme
                }
            }
            else
            {
                speedParameter = 0f; // Dursun
            }
            
            animator.SetFloat(SPEED_PARAMETER, speedParameter);
            animator.SetBool(IS_GROUNDED, isGrounded);
        }
    }

    private void Jump()
    {
        if (rb != null && isGrounded)
        {
            // Zıplama gücü
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            if (animator != null)
            {
                animator.SetTrigger(IS_JUMPING);
            }
            
            isGrounded = false;
        }
    }

    private void CheckGrounded()
    {
        // Karakterin altında raycast atarak yerden ayrılıp ayrılmadığını kontrol et
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        
        bool wasGrounded = isGrounded;
        
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        // Debug görselleştirme (Scene view'da görmek için)
        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, rayColor);
    }
}
