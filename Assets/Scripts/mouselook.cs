using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Varsayılan değer 100, ama oyun açılınca PlayerPrefs'ten üzerine yazacağız.
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    // SettingsManager'da kullandığımız anahtarın aynısı olmalı!
    private const string KEY_SENSITIVITY = "Sensitivity";

    void Start()
    {
        // 1. ÖNCEKİ KAYDI YÜKLE
        // Eğer kayıt yoksa varsayılan olarak 100f kullan.
        mouseSensitivity = PlayerPrefs.GetFloat(KEY_SENSITIVITY, 100f);

        // Fareyi kilitle
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Burası aynı kalıyor
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}