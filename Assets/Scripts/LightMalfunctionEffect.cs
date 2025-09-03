using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class LightMalfunctionEffect : MonoBehaviour
{
    [Header("Фаза 1: Первоначальные блики")]
    [Tooltip("Сколько раз свет мигнет, прежде чем погаснуть.")]
    public int initialBlinks = 3;
    [Tooltip("Как быстро происходят блики (пауза между включением/выключением).")]
    public float blinkSpeed = 0.1f;

    [Header("Фаза 2: Пауза")]
    [Tooltip("На сколько секунд свет полностью погаснет после бликов.")]
    public float deadTime = 2.0f;

    [Header("Фаза 3: Финальное мерцание")]
    [Tooltip("Скорость финального 'сломанного' мерцания.")]
    public float flickerSpeed = 20f;
    [Tooltip("Минимальная яркость при мерцании (в % от изначальной).")]
    [Range(0f, 1f)]
    public float minFlickerIntensity = 0.2f;
    [Tooltip("Максимальная яркость при мерцании (в % от изначальной).")]
    [Range(1f, 2f)]
    public float maxFlickerIntensity = 1.1f;

    [Header("Звук")]
    [Tooltip("Звук, который проигрывается в момент начала 'поломки'.")]
    public AudioClip malfunctionSound;

    // Приватные переменные
    private Light lightComponent;
    private AudioSource audioSource;
    private float initialIntensity;
    private bool isFlickering = false;
    private float randomOffset;
    private bool hasBeenTriggered = false; // Флаг, чтобы эффект не запускался повторно

    void Awake()
    {
        lightComponent = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        initialIntensity = lightComponent.intensity;
        randomOffset = Random.Range(0f, 1000f);

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!isFlickering)
        {
            return;
        }

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
        float targetIntensity = Mathf.Lerp(
            initialIntensity * minFlickerIntensity,
            initialIntensity * maxFlickerIntensity,
            noise
        );
        lightComponent.intensity = targetIntensity;
    }

    /// <summary>
    /// ПУБЛИЧНЫЙ МЕТОД: Запускает всю последовательность эффекта.
    /// Вызовите его из вашего триггера (TriggerEvent) или другого UnityEvent.
    /// </summary>
    public void StartEffect()
    {
        // Если эффект уже был запущен, ничего не делаем
        if (hasBeenTriggered)
        {
            return;
        }
        hasBeenTriggered = true; // Помечаем, что эффект запущен

        // Запускаем корутину, которая управляет всей последовательностью
        StartCoroutine(MalfunctionRoutine());
    }

    private IEnumerator MalfunctionRoutine()
    {
        // --- ПРОИГРЫВАЕМ ЗВУК В САМОМ НАЧАЛЕ ---
        if (malfunctionSound != null)
        {
            audioSource.PlayOneShot(malfunctionSound);
        }

        // --- ФАЗА 1: БЛИКИ ---
        for (int i = 0; i < initialBlinks; i++)
        {
            lightComponent.intensity = 0f;
            yield return new WaitForSeconds(blinkSpeed);
            lightComponent.intensity = initialIntensity;
            yield return new WaitForSeconds(blinkSpeed);
        }

        // --- ФАЗА 2: ПАУЗА ---
        lightComponent.intensity = 0f;
        yield return new WaitForSeconds(deadTime);

        // --- НАЧАЛО ФАЗЫ 3 ---
        isFlickering = true;
    }
}