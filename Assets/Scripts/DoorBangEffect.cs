using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DoorBangEffect : MonoBehaviour
{
    // ... (все ваши поля остаются без изменений) ...
    public Transform doorToShake;
    public AudioClip[] bangSounds;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 3.0f;
    public UnityEvent OnEffectFinished;
    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Awake()
    {
        // --- ДОБАВЬТЕ ЭТУ СТРОКУ ---
        // Эта строка напишет в консоль имя объекта, на котором запущен этот скрипт.
        Debug.Log("!!! Скрипт DoorBangEffect ЗАПУЩЕН на объекте: " + gameObject.name, this);

        if (doorToShake != null)
        {
            audioSource = doorToShake.GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError($"На двери '{doorToShake.name}' не найден компонент AudioSource! Эффект отключен.", this);
            enabled = false;
        }
    }

    // ... (остальной код остается без изменений) ...

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(ShakeDoorRoutine());
        }
    }

    private IEnumerator ShakeDoorRoutine()
    {
        if (bangSounds != null && bangSounds.Length > 0)
        {
            AudioClip clipToPlay = bangSounds[Random.Range(0, bangSounds.Length)];
            audioSource.PlayOneShot(clipToPlay);
        }

        Quaternion originalRotation = doorToShake.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            float z = Random.Range(-1f, 1f) * shakeIntensity;
            doorToShake.localRotation = originalRotation * Quaternion.Euler(x, y, z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorToShake.localRotation = originalRotation;
        if (OnEffectFinished != null)
        {
            OnEffectFinished.Invoke();
        }
    }
}