using UnityEngine;
using System.Collections.Generic;

// Убедимся, что на объекте есть AudioSource для проигрывания звуков
[RequireComponent(typeof(AudioSource))]
public class XRFootstepPlayer : MonoBehaviour
{
    [Header("Цель для отслеживания")]
    [Tooltip("Перетащите сюда главный объект камеры игрока (Main Camera). Его позиция будет отслеживаться.")]
    public Transform playerHeadTransform;

    [Header("Настройки шагов")]
    [Tooltip("Список звуков шагов. Будет проигрываться случайный звук из этого списка.")]
    public List<AudioClip> footstepSounds;

    [Tooltip("Какое расстояние в метрах должен пройти игрок, чтобы прозвучал следующий шаг.")]
    public float stepDistance = 0.8f;

    [Tooltip("Громкость звуков шагов (от 0.0 до 1.0).")]
    [Range(0f, 1f)]
    public float volume = 0.5f;

    // Приватные переменные для работы скрипта
    private AudioSource audioSource;
    private Vector3 lastStepPosition;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Важная проверка: если не указана голова игрока, скрипт не будет работать
        if (playerHeadTransform == null)
        {
            Debug.LogError("Не назначен 'playerHeadTransform'! Скрипт шагов не будет работать. Перетащите Main Camera в инспектор.", this);
            enabled = false; // Выключаем скрипт, чтобы не вызывать ошибок в Update
            return;
        }

        // Запоминаем начальную позицию игрока
        lastStepPosition = playerHeadTransform.position;
    }

    void Update()
    {
        // Если что-то не настроено, ничего не делаем
        if (playerHeadTransform == null || footstepSounds.Count == 0)
        {
            return;
        }

        // --- КЛЮЧЕВАЯ ЛОГИКА ---

        // Мы используем только X и Z координаты, чтобы игнорировать движение вверх/вниз (прыжки, приседания)
        Vector3 currentPositionXZ = new Vector3(playerHeadTransform.position.x, 0, playerHeadTransform.position.z);
        Vector3 lastPositionXZ = new Vector3(lastStepPosition.x, 0, lastStepPosition.z);

        // Считаем, какое расстояние прошел игрок по горизонтали с момента последнего шага
        float distanceMoved = Vector3.Distance(currentPositionXZ, lastPositionXZ);

        // Если пройденное расстояние больше или равно порогу...
        if (distanceMoved >= stepDistance)
        {
            // ...проигрываем звук шага!
            PlayFootstepSound();

            // И обновляем позицию последнего шага на текущую позицию
            lastStepPosition = playerHeadTransform.position;
        }
    }

    private void PlayFootstepSound()
    {
        // Выбираем случайный клип из списка
        AudioClip clipToPlay = footstepSounds[Random.Range(0, footstepSounds.Count)];

        // Проигрываем его с заданной громкостью
        audioSource.PlayOneShot(clipToPlay, volume);
    }
}