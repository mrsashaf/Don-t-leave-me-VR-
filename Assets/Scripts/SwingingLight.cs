using UnityEngine;

/// <summary>
/// Заставляет объект качаться, изменяя его локальные углы Эйлера.
/// Этот метод более прямолинеен и может избежать проблем с вращением родительских объектов.
/// Поместите этот скрипт на родительский объект-крепление.
/// Все дочерние объекты (лампа, свет) будут качаться вместе с ним.
/// </summary>
public class SwingingLight : MonoBehaviour
{
    public enum SwingAxis { X, Z }

    [Header("Настройки качания")]
    [Tooltip("Максимальный угол отклонения от центра в градусах.")]
    [Range(0f, 90f)]
    public float maxSwingAngle = 15f;

    [Tooltip("Скорость качания.")]
    public float swingSpeed = 1f;

    [Tooltip("Ось, по которой будет происходить качание.")]
    public SwingAxis swingAxis = SwingAxis.Z;

    [Header("Случайность")]
    [Tooltip("Добавляет случайное смещение, чтобы лампы не качались синхронно.")]
    public bool addRandomness = true;

    // Приватные переменные
    private Vector3 initialLocalAngles;
    private float randomOffset = 0f;

    void Start()
    {
        // 1. Запоминаем изначальные ЛОКАЛЬНЫЕ углы объекта.
        initialLocalAngles = transform.localEulerAngles;

        // 2. Генерируем смещение для асинхронности
        if (addRandomness)
        {
            randomOffset = Random.Range(0f, 100f);
        }
    }

    void Update()
    {
        // 1. Вычисляем текущее смещение угла с помощью синусоиды.
        // Это создает плавное колебание от -maxSwingAngle до +maxSwingAngle.
        float swingOffset = maxSwingAngle * Mathf.Sin(Time.time * swingSpeed + randomOffset);

        // ВАЖНО: Для диагностики. Проверьте консоль (Window -> General -> Console).
        // Если вы видите эти сообщения, значит Update() работает.
        // Debug.Log("Качание. Смещение угла: " + swingOffset);

        // 2. Создаем новый вектор для целевых углов, начиная с исходных.
        Vector3 targetLocalAngles = initialLocalAngles;

        // 3. Применяем смещение к нужной оси.
        if (swingAxis == SwingAxis.X)
        {
            targetLocalAngles.x += swingOffset;
        }
        else // swingAxis == SwingAxis.Z
        {
            targetLocalAngles.z += swingOffset;
        }

        // 4. Напрямую устанавливаем вычисленные локальные углы объекту.
        // Так как мы меняем transform родителя, все дочерние объекты последуют за ним.
        transform.localEulerAngles = targetLocalAngles;
    }
}