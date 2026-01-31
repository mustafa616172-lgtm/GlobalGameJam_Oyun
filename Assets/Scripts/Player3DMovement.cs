using UnityEngine;

public class Player3DMovement : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 7f;
    public float jumpForce = 5f;
    
    [Header("Yer Kontrolü")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform groundCheck;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        // Karakterin devrilmemesi için rotasyonu donduruyoruz
        rb.freezeRotation = true; 
    }

    void Update()
    {
        // Yer kontrolü
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Zıplama (Space)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }
        
        // Yere değdiğinde jumping parametresini sıfırla
        if (isGrounded && animator != null)
        {
            animator.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        // WASD veya Ok tuşlarından input al (-1 ile 1 arası)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Hareket yönünü hesapla (Karakterin baktığı yöne göre)
        Vector3 move = transform.right * x + transform.forward * z;

        // Rigidbody ile hareket uygula
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        
        // Animator parametrelerini güncelle
        if (animator != null)
        {
            // Hareket hızını hesapla (0-10 arası normalize edilmiş değer)
            float currentSpeed = rb.linearVelocity.magnitude;
            animator.SetFloat("Speed", currentSpeed);
        }
    }
}