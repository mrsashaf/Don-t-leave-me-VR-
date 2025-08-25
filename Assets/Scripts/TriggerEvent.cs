using UnityEngine;
using UnityEngine.Events; // ОБЯЗАТЕЛЬНО: для работы с UnityEvent

// [RequireComponent] гарантирует, что на объекте всегда будет коллайдер.
// Это защищает от случайного удаления коллайдера, без которого скрипт бесполезен.
[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [Header("Настройки триггера")]
    [Tooltip("Тег объекта, который должен вызывать событие. Чаще всего это 'Player'. Оставьте пустым, чтобы реагировать на любой объект.")]
    public string triggerTag = "Player";

    [Tooltip("Если эта галочка стоит, триггер сработает только один раз и больше не будет активен.")]
    public bool fireOnce = true;

    [Header("Событие")]
    [Tooltip("Это событие будет вызвано, когда объект с нужным тегом войдет в триггер.")]
    public UnityEvent OnTriggered;

    // Приватная переменная, чтобы отслеживать, сработал ли уже триггер
    private bool hasFired = false;

    private void OnTriggerEnter(Collider other)
    {
        // --- ПРОВЕРКИ ---

        // 1. Если триггер одноразовый и он уже сработал, ничего не делаем.
        if (fireOnce && hasFired)
        {
            return;
        }

        // 2. Проверяем, совпадает ли тег вошедшего объекта с тем, что нам нужен.
        // Если поле triggerTag пустое, мы пропускаем эту проверку и реагируем на всё.
        bool isTagValid = string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag);

        // Если тег не совпадает, ничего не делаем.
        if (!isTagValid)
        {
            return;
        }

        // --- ВЫПОЛНЕНИЕ ---

        // Если все проверки пройдены, выводим сообщение в консоль для отладки
        Debug.Log($"Триггер '{gameObject.name}' активирован объектом '{other.name}'.");

        // Вызываем все функции, которые вы настроили в инспекторе
        OnTriggered.Invoke();

        // Если триггер одноразовый, помечаем, что он сработал
        if (fireOnce)
        {
            hasFired = true;
        }
    }

    // Этот метод вызывается в редакторе, чтобы показать зону триггера для удобства
    private void OnDrawGizmos()
    {
        // Рисуем полупрозрачный зеленый куб, который показывает границы триггера
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        // Пытаемся взять размер из BoxCollider, если он есть
        var boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
    }
}