using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Fare hassasiyeti
    public Transform playerBody; // Karakterin kendisi (sağa sola dönecek)

    float xRotation = 0f; // Yukarı aşağı bakma açısı

    void Start()
    {
        // Mouse imlecini ekrana kilitler ve gizler
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Fare hareketlerini al
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Yukarı-Aşağı bakma (Clamping - boyun kırma engelleme)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Kamerayı yukarı-aşağı döndür
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Karakterin GÖVDESİNİ sağa-sola döndür (Böylece W ileri götürür)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}