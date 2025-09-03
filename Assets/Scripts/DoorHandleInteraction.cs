using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class DoorHandleInteraction : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Ссылки на Объекты")]
    public Transform handleToRotate;
    public Transform doorToShake;

    [Header("Общие Настройки")]
    public float totalInteractionDuration = 4.0f;
    [Range(0f, 1f)]
    public float knockChance = 0.4f;

    [Header("Настройки Поворота Ручки")]
    public Vector3 handleRotationAxis = Vector3.forward;
    public float maxHandleRotation = 45.0f;
    public float minHandleMoveDuration = 0.1f;
    public float maxHandleMoveDuration = 0.3f;
    public List<AudioClip> handleRattleSounds = new List<AudioClip>();

    [Header("Настройки Удара в Дверь")]
    public Vector3 doorShakeAxis = Vector3.up;
    public float doorShakeAngle = 5.0f;
    public float doorShakeDuration = 0.15f;
    public List<AudioClip> doorKnockSounds = new List<AudioClip>();

    [Header("Финальный Звук")]
    public AudioClip releaseSound;

    [Header("События")]
    public UnityEvent onInteractionCompleted;

    private Quaternion originalHandleRotation;
    private Quaternion originalDoorRotation;
    private bool isInteracting = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (handleToRotate != null) originalHandleRotation = handleToRotate.localRotation;
        if (doorToShake != null) originalDoorRotation = doorToShake.localRotation;
    }

    public void PerformInteraction()
    {
        if (isInteracting) return;
        StartCoroutine(InteractionRoutine());
    }

    private IEnumerator InteractionRoutine()
    {
        isInteracting = true;
        float mainTimer = 0f;

        while (mainTimer < totalInteractionDuration)
        {
            if (Random.value < knockChance)
            {
                yield return StartCoroutine(KnockDoorRoutine());
                mainTimer += doorShakeDuration + 0.1f;
            }
            else
            {
                float handleActionDuration = 0f;
                yield return StartCoroutine(RattleHandleRoutine(duration => handleActionDuration = duration));
                mainTimer += handleActionDuration;
            }
        }

        yield return StartCoroutine(ReturnToIdleRoutine());
        onInteractionCompleted.Invoke();
        isInteracting = false;
    }

    private IEnumerator RattleHandleRoutine(System.Action<float> onCompleted)
    {
        if (handleToRotate == null) { onCompleted(0); yield break; }
        float totalDuration = 0f;
        Quaternion lastRotation = handleToRotate.localRotation;
        float randomTargetAngle = Random.Range(-maxHandleRotation * 0.5f, maxHandleRotation);
        Quaternion targetRotation = originalHandleRotation * Quaternion.AngleAxis(randomTargetAngle, handleRotationAxis);
        float moveDuration = Random.Range(minHandleMoveDuration, maxHandleMoveDuration);
        totalDuration += moveDuration;
        float moveTimer = 0f;

        // --- ВОТ ИСПРАВЛЕНИЕ ---
        while (moveTimer < moveDuration) // Было: moveTimer < moveTimer
        {
            handleToRotate.localRotation = Quaternion.Slerp(lastRotation, targetRotation, moveTimer / moveDuration);
            moveTimer += Time.deltaTime;
            yield return null;
        }

        handleToRotate.localRotation = targetRotation;
        if (handleRattleSounds.Count > 0)
        {
            AudioClip clip = handleRattleSounds[Random.Range(0, handleRattleSounds.Count)];
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
            totalDuration += clip.length;
        }
        onCompleted(totalDuration);
    }

    // ... (остальные корутины остаются без изменений) ...
    private IEnumerator KnockDoorRoutine()
    {
        if (doorToShake == null) yield break;
        if (doorKnockSounds.Count > 0) { audioSource.PlayOneShot(doorKnockSounds[Random.Range(0, doorKnockSounds.Count)]); }
        Quaternion knockedRotation = originalDoorRotation * Quaternion.AngleAxis(doorShakeAngle, doorShakeAxis);
        float timer = 0f;
        while (timer < doorShakeDuration / 2)
        {
            doorToShake.localRotation = Quaternion.Slerp(originalDoorRotation, knockedRotation, timer / (doorShakeDuration / 2));
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        while (timer < doorShakeDuration / 2)
        {
            doorToShake.localRotation = Quaternion.Slerp(knockedRotation, originalDoorRotation, timer / (doorShakeDuration / 2));
            timer += Time.deltaTime;
            yield return null;
        }
        doorToShake.localRotation = originalDoorRotation;
    }
    private IEnumerator ReturnToIdleRoutine()
    {
        if (releaseSound != null) { audioSource.PlayOneShot(releaseSound); }
        float returnDuration = 0.4f;
        float returnTimer = 0f;
        Quaternion finalHandleRotation = handleToRotate != null ? handleToRotate.localRotation : originalHandleRotation;
        while (returnTimer < returnDuration)
        {
            if (handleToRotate != null) handleToRotate.localRotation = Quaternion.Slerp(finalHandleRotation, originalHandleRotation, returnTimer / returnDuration);
            returnTimer += Time.deltaTime;
            yield return null;
        }
        if (handleToRotate != null) handleToRotate.localRotation = originalHandleRotation;
    }
}