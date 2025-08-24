using UnityEngine;
using System.Collections;

public class DoorRotationController : MonoBehaviour
{
    [Header("Настройки вращения")]
    public float rotationSpeed = 2.0f;
    public float openAngle = 90.0f;
    [Tooltip("Если отмечено, дверь открывается в сторону положительного угла. Если нет - в противоположную.")]
    public bool opensInDefaultDirection = true;

    private Quaternion initialRotation;
    private Quaternion openRotation;

    // ИЗМЕНЕНИЕ: Сделали переменную публичной (public)
    public bool isPlayerInRange = false;

    void Start()
    {
        initialRotation = transform.rotation;
        float finalOpenAngle = opensInDefaultDirection ? openAngle : -openAngle;
        openRotation = initialRotation * Quaternion.Euler(0, finalOpenAngle, 0);
    }

    void Update()
    {
        Quaternion targetRotation = isPlayerInRange ? openRotation : initialRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}