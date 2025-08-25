using UnityEngine;

public class SwingingLight : MonoBehaviour
{
    public enum SwingAxis { X, Z } // Оси, по которым может происходить качание

    [Header("Настройки качания")]
    [Tooltip("Максимальный угол качания в градусах в каждую сторону.")]
    [Range(0f, 90f)]
    public float maxSwingAngle = 15f;

    [Tooltip("Скорость качания. Чем выше значение, тем быстрее.")]
    public float swingSpeed = 1f;

    [Tooltip("Ось, по которой будет качаться лампа (X = вперед-назад, Z = вправо-влево).")]
    public SwingAxis swingAxis = SwingAxis.Z;

    [Header("Случайность")]
    [Tooltip("Добавляет небольшую случайность в скорость и угол, чтобы лампы не качались синхронно.")]
    public bool addRandomness = true;

    // Приватные переменные для хранения начальных значений
    private Quaternion initialRotation;
    private float randomOffset;

    void Start()
    {
        // Запоминаем изначальный поворот объекта
        initialRotation = transform.rotation;

        // Если включена случайность, добавляем уникальное смещение для этой лампы
        if (addRandomness)
        {
            // Случайное число, которое сделает движение уникальным
            randomOffset = Random.Range(0f, 100f);
        }
    }

    void Update()
    {
        // 1. Вычисляем базовый угол с помощью синусоиды
        // Mathf.Sin(time) создает плавное колебание между -1 и 1
        float time = Time.time * swingSpeed + randomOffset;
        float currentAngle = maxSwingAngle * Mathf.Sin(time);

        // 2. Создаем вращение на основе вычисленного угла
        Quaternion swingRotation;

        // Выбираем ось вращения в зависимости от настройки в инспекторе
        if (swingAxis == SwingAxis.X)
        {
            // Вращение вперед-назад
            swingRotation = Quaternion.Euler(currentAngle, 0f, 0f);
        }
        else // swingAxis == SwingAxis.Z
        {
            // Вращение вправо-влево
            swingRotation = Quaternion.Euler(0f, 0f, currentAngle);
        }

        // 3. Применяем новое вращение к изначальному
        // Мы умножаем, чтобы применить локальное качание к исходному мировому повороту
        transform.rotation = initialRotation * swingRotation;
    }
}