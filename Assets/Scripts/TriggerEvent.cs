using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [Header("Настройки триггера")]
    [Tooltip("Тег объекта, который должен вызывать событие. Оставьте пустым, чтобы реагировать на любой объект.")]
    public string triggerTag = "Player";

    [Tooltip("Если включено, событие сработает один раз.")]
    public bool fireOnce = true;

    [Header("Событие")]
    [Tooltip("Вызовется при входе нужного объекта в триггер.")]
    public UnityEvent OnTriggered;

    private bool hasFired = false;

    private void OnTriggerEnter(Collider other)
    {
        if (fireOnce && hasFired) return;

        bool isTagValid = string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag);
        if (!isTagValid) return;

        Debug.Log($"Триггер '{gameObject.name}' активирован объектом '{other.name}'.");

        OnTriggered?.Invoke();

        if (fireOnce) hasFired = true;
    }

    // ---------------- GIZMOS ----------------

    [Header("Gizmos (визуализация триггера)")]
    [Tooltip("Показывать гизмо зоны триггера в сцене.")]
    public bool showGizmos = true;

    [Tooltip("Цвет гизмо (учитывается альфа).")]
    public Color gizmoColor = new Color(0f, 1f, 0f, 0.3f);

    [Tooltip("Цвет при выделении объекта в иерархии.")]
    public Color gizmoColorSelected = new Color(0f, 1f, 0f, 0.5f);

    [Tooltip("Рисовать каркас вместо заполненной формы.")]
    public bool wireframe = false;

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        DrawColliderGizmo(gizmoColor);
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        DrawColliderGizmo(gizmoColorSelected);
    }

    private void DrawColliderGizmo(Color color)
    {
        var col = GetComponent<Collider>();
        if (col == null) return;

        Gizmos.color = color;
        Gizmos.matrix = transform.localToWorldMatrix;

        if (col is BoxCollider box)
        {
            if (wireframe) Gizmos.DrawWireCube(box.center, box.size);
            else Gizmos.DrawCube(box.center, box.size);
        }
        else if (col is SphereCollider sphere)
        {
            // Для сферы учитываем localScale по максимальной оси (как делает Unity)
            var maxScale = Mathf.Max(
                Mathf.Abs(transform.lossyScale.x),
                Mathf.Abs(transform.lossyScale.y),
                Mathf.Abs(transform.lossyScale.z)
            );

            // Так как уже выставили matrix = localToWorld, рисуем в локальных координатах
            var center = sphere.center;
            var radius = sphere.radius;

            // У Gizmos нет DrawSphere c матрицей радиуса, поэтому "масштабируем" через матрицу вручную:
            // Временная матрица: перенос в центр и масштаб по радиусу и maxScale.
            var prev = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.TRS(center, Quaternion.identity, Vector3.one * (radius * 2f));
            if (wireframe) Gizmos.DrawWireSphere(Vector3.zero, 0.5f); // сфера единичного диаметра
            else Gizmos.DrawSphere(Vector3.zero, 0.5f);
            Gizmos.matrix = prev;
        }
        else
        {
            // Фолбэк: рисуем bounds как коробку в мировых координатах
            // Для этого временно сбросим матрицу, т.к. bounds уже в world space.
            var prev = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.identity;

            var b = col.bounds;
            if (wireframe) Gizmos.DrawWireCube(b.center, b.size);
            else Gizmos.DrawCube(b.center, b.size);

            Gizmos.matrix = prev;
        }
    }
}
