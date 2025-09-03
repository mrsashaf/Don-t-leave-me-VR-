using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("��� �������, ������� ������ �������� �������. �������� ������, ����� ����������� �� ����� ������.")]
    public string triggerTag = "Player";

    [Tooltip("���� ��������, ������� ��������� ���� ���.")]
    public bool fireOnce = true;

    [Header("�������")]
    [Tooltip("��������� ��� ����� ������� ������� � �������.")]
    public UnityEvent OnTriggered;

    private bool hasFired = false;

    private void OnTriggerEnter(Collider other)
    {
        if (fireOnce && hasFired) return;

        bool isTagValid = string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag);
        if (!isTagValid) return;

        Debug.Log($"������� '{gameObject.name}' ����������� �������� '{other.name}'.");

        OnTriggered?.Invoke();

        if (fireOnce) hasFired = true;
    }

    // ---------------- GIZMOS ----------------

    [Header("Gizmos (������������ ��������)")]
    [Tooltip("���������� ����� ���� �������� � �����.")]
    public bool showGizmos = true;

    [Tooltip("���� ����� (����������� �����).")]
    public Color gizmoColor = new Color(0f, 1f, 0f, 0.3f);

    [Tooltip("���� ��� ��������� ������� � ��������.")]
    public Color gizmoColorSelected = new Color(0f, 1f, 0f, 0.5f);

    [Tooltip("�������� ������ ������ ����������� �����.")]
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
            // ��� ����� ��������� localScale �� ������������ ��� (��� ������ Unity)
            var maxScale = Mathf.Max(
                Mathf.Abs(transform.lossyScale.x),
                Mathf.Abs(transform.lossyScale.y),
                Mathf.Abs(transform.lossyScale.z)
            );

            // ��� ��� ��� ��������� matrix = localToWorld, ������ � ��������� �����������
            var center = sphere.center;
            var radius = sphere.radius;

            // � Gizmos ��� DrawSphere c �������� �������, ������� "������������" ����� ������� �������:
            // ��������� �������: ������� � ����� � ������� �� ������� � maxScale.
            var prev = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.TRS(center, Quaternion.identity, Vector3.one * (radius * 2f));
            if (wireframe) Gizmos.DrawWireSphere(Vector3.zero, 0.5f); // ����� ���������� ��������
            else Gizmos.DrawSphere(Vector3.zero, 0.5f);
            Gizmos.matrix = prev;
        }
        else
        {
            // ������: ������ bounds ��� ������� � ������� �����������
            // ��� ����� �������� ������� �������, �.�. bounds ��� � world space.
            var prev = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.identity;

            var b = col.bounds;
            if (wireframe) Gizmos.DrawWireCube(b.center, b.size);
            else Gizmos.DrawCube(b.center, b.size);

            Gizmos.matrix = prev;
        }
    }
}
