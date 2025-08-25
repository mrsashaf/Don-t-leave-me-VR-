using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DoorBangEffect : MonoBehaviour
{
    // ... (��� ���� ���� �������� ��� ���������) ...
    public Transform doorToShake;
    public AudioClip[] bangSounds;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 3.0f;
    public UnityEvent OnEffectFinished;
    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Awake()
    {
        // --- �������� ��� ������ ---
        // ��� ������ ������� � ������� ��� �������, �� ������� ������� ���� ������.
        Debug.Log("!!! ������ DoorBangEffect ������� �� �������: " + gameObject.name, this);

        if (doorToShake != null)
        {
            audioSource = doorToShake.GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError($"�� ����� '{doorToShake.name}' �� ������ ��������� AudioSource! ������ ��������.", this);
            enabled = false;
        }
    }

    // ... (��������� ��� �������� ��� ���������) ...

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