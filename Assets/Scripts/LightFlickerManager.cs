using UnityEngine;
using System.Collections.Generic;

public class LightFlickerManager : MonoBehaviour
{
    [Header("Целевые объекты")]
    [Tooltip("Перетащите сюда игровые объекты, содержащие светильники (например, модели ламп). Скрипт сам найдет компонент Light.")]
    public List<GameObject> lightGameObjects = new List<GameObject>();

    [Header("Параметры мерцания")]
    [Tooltip("Как быстро происходит мерцание. Чем выше значение, тем быстрее.")]
    [Range(0.1f, 50f)]
    public float flickerSpeed = 10f;

    [Tooltip("Минимальная яркость в процентах от изначальной (например, 0.5 = 50%).")]
    [Range(0f, 1f)]
    public float minIntensityMultiplier = 0.5f;

    [Tooltip("Максимальная яркость в процентах от изначальной (например, 1.5 = 150%).")]
    [Range(1f, 3f)]
    public float maxIntensityMultiplier = 1.2f;

    // --- Внутренние переменные, которые теперь работают со найденными светильниками ---
    private List<Light> foundLights = new List<Light>(); // Список для хранения НАЙДЕННЫХ светильников
    private Dictionary<Light, float> initialIntensities;
    private Dictionary<Light, float> randomOffsets;

    void Awake()
    {
        // Инициализируем словари и список
        initialIntensities = new Dictionary<Light, float>();
        randomOffsets = new Dictionary<Light, float>();
        foundLights = new List<Light>();

        // Проходим по списку игровых объектов, которые вы указали в инспекторе
        foreach (GameObject containerObject in lightGameObjects)
        {
            if (containerObject == null) continue; // Пропускаем пустые слоты в списке

            // --- НОВАЯ ЛОГИКА ПОИСКА ---
            // Ищем компонент Light в самом объекте ИЛИ в его дочерних объектах.
            // Это самый надежный способ.
            Light lightSource = containerObject.GetComponentInChildren<Light>();

            // Если светильник был найден...
            if (lightSource != null)
            {
                // Добавляем его в наш рабочий список
                foundLights.Add(lightSource);

                // И выполняем ту же логику, что и раньше: запоминаем яркость и создаем смещение
                initialIntensities.Add(lightSource, lightSource.intensity);
                randomOffsets.Add(lightSource, Random.Range(0f, 1000f));
            }
            // Если светильник НЕ был найден...
            else
            {
                // Выводим в консоль предупреждение, чтобы вы знали, какой объект настроен неверно
                Debug.LogWarning($"В объекте '{containerObject.name}' не найден компонент 'Light'. Он не будет мерцать.", containerObject);
            }
        }
    }

    void Update()
    {
        // Теперь мы работаем с нашим внутренним списком НАЙДЕННЫХ светильников
        foreach (Light lightSource in foundLights)
        {
            if (lightSource != null && lightSource.enabled)
            {
                float initialIntensity = initialIntensities[lightSource];
                float offset = randomOffsets[lightSource];

                float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, offset);

                float targetIntensity = Mathf.Lerp(
                    initialIntensity * minIntensityMultiplier,
                    initialIntensity * maxIntensityMultiplier,
                    noise
                );

                lightSource.intensity = targetIntensity;
            }
        }
    }
}