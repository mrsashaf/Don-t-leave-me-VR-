using UnityEngine;
using UnityEngine.Events; // �����������: ��� ������ � UnityEvent

// [RequireComponent] �����������, ��� �� ������� ������ ����� ���������.
// ��� �������� �� ���������� �������� ����������, ��� �������� ������ ����������.
[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("��� �������, ������� ������ �������� �������. ���� ����� ��� 'Player'. �������� ������, ����� ����������� �� ����� ������.")]
    public string triggerTag = "Player";

    [Tooltip("���� ��� ������� �����, ������� ��������� ������ ���� ��� � ������ �� ����� �������.")]
    public bool fireOnce = true;

    [Header("�������")]
    [Tooltip("��� ������� ����� �������, ����� ������ � ������ ����� ������ � �������.")]
    public UnityEvent OnTriggered;

    // ��������� ����������, ����� �����������, �������� �� ��� �������
    private bool hasFired = false;

    private void OnTriggerEnter(Collider other)
    {
        // --- �������� ---

        // 1. ���� ������� ����������� � �� ��� ��������, ������ �� ������.
        if (fireOnce && hasFired)
        {
            return;
        }

        // 2. ���������, ��������� �� ��� ��������� ������� � ���, ��� ��� �����.
        // ���� ���� triggerTag ������, �� ���������� ��� �������� � ��������� �� ��.
        bool isTagValid = string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag);

        // ���� ��� �� ���������, ������ �� ������.
        if (!isTagValid)
        {
            return;
        }

        // --- ���������� ---

        // ���� ��� �������� ��������, ������� ��������� � ������� ��� �������
        Debug.Log($"������� '{gameObject.name}' ����������� �������� '{other.name}'.");

        // �������� ��� �������, ������� �� ��������� � ����������
        OnTriggered.Invoke();

        // ���� ������� �����������, ��������, ��� �� ��������
        if (fireOnce)
        {
            hasFired = true;
        }
    }

    // ���� ����� ���������� � ���������, ����� �������� ���� �������� ��� ��������
    private void OnDrawGizmos()
    {
        // ������ �������������� ������� ���, ������� ���������� ������� ��������
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        // �������� ����� ������ �� BoxCollider, ���� �� ����
        var boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
    }
}