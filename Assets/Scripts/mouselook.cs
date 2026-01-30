using UnityEngine;

public partial class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Fareyi oyunun ortasına kilitler ve gizler
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        // Kafayı çok fazla geriye veya aşağıya bükmemek için sınırlandırıyoruz (Clamp)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Gövdeyi sağa sola döndürür
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
