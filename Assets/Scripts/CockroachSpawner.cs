using System.Collections;
using UnityEngine;

public class CockroachSpawner : MonoBehaviour
{
    // --- ВОТ ИСПРАВЛЕНИЕ, ЧАСТЬ 1 ---
    // Создаем статическую переменную, чтобы к ней можно было обращаться из любого другого скрипта
    public static CockroachSpawner instance;

    [Header("Ссылки на объекты")]
    public GameObject cockroachPrefab;
    public Transform spawnPoint;

    [Header("Настройки волны")]
    public int cockroachesInWave = 25;
    public float timeBetweenSpawns = 0.1f;

    [Header("Цели для тараканов")]
    public Transform[] hidingSpots;

    // --- ВОТ ИСПРАВЛЕНИЕ, ЧАСТЬ 2 ---
    // Awake() вызывается при загрузке скрипта, еще до старта игры
    void Awake()
    {
        // Проверяем, не была ли переменная instance уже кем-то занята
        if (instance == null)
        {
            // Если нет, то "я" (этот конкретный объект спаунера) становлюсь главным
            instance = this;
        }
        else
        {
            // Если в сцене уже есть другой спаунер, этот удаляется, чтобы не было конфликтов
            Destroy(gameObject);
        }
    }

    // Публичная функция для вызова волны
    public void TriggerWave()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    // Функция для получения случайного укрытия
    public Transform GetRandomSpot()
    {
        if (hidingSpots.Length == 0)
        {
            Debug.LogError("В спаунере не указано ни одного укрытия (Hiding Spot)!");
            return null;
        }
        int randomIndex = Random.Range(0, hidingSpots.Length);
        return hidingSpots[randomIndex];
    }

    // Корутина для создания волны
    IEnumerator SpawnWaveCoroutine()
    {
        for (int i = 0; i < cockroachesInWave; i++)
        {
            Instantiate(cockroachPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}