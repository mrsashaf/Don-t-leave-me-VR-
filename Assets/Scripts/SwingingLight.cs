using UnityEngine;

/// <summary>
/// ���������� ������ ��������, ������� ��� ��������� ���� ������.
/// ���� ����� ����� ����������� � ����� �������� ������� � ��������� ������������ ��������.
/// ��������� ���� ������ �� ������������ ������-���������.
/// ��� �������� ������� (�����, ����) ����� �������� ������ � ���.
/// </summary>
public class SwingingLight : MonoBehaviour
{
    public enum SwingAxis { X, Z }

    [Header("��������� �������")]
    [Tooltip("������������ ���� ���������� �� ������ � ��������.")]
    [Range(0f, 90f)]
    public float maxSwingAngle = 15f;

    [Tooltip("�������� �������.")]
    public float swingSpeed = 1f;

    [Tooltip("���, �� ������� ����� ����������� �������.")]
    public SwingAxis swingAxis = SwingAxis.Z;

    [Header("�����������")]
    [Tooltip("��������� ��������� ��������, ����� ����� �� �������� ���������.")]
    public bool addRandomness = true;

    // ��������� ����������
    private Vector3 initialLocalAngles;
    private float randomOffset = 0f;

    void Start()
    {
        // 1. ���������� ����������� ��������� ���� �������.
        initialLocalAngles = transform.localEulerAngles;

        // 2. ���������� �������� ��� �������������
        if (addRandomness)
        {
            randomOffset = Random.Range(0f, 100f);
        }
    }

    void Update()
    {
        // 1. ��������� ������� �������� ���� � ������� ���������.
        // ��� ������� ������� ��������� �� -maxSwingAngle �� +maxSwingAngle.
        float swingOffset = maxSwingAngle * Mathf.Sin(Time.time * swingSpeed + randomOffset);

        // �����: ��� �����������. ��������� ������� (Window -> General -> Console).
        // ���� �� ������ ��� ���������, ������ Update() ��������.
        // Debug.Log("�������. �������� ����: " + swingOffset);

        // 2. ������� ����� ������ ��� ������� �����, ������� � ��������.
        Vector3 targetLocalAngles = initialLocalAngles;

        // 3. ��������� �������� � ������ ���.
        if (swingAxis == SwingAxis.X)
        {
            targetLocalAngles.x += swingOffset;
        }
        else // swingAxis == SwingAxis.Z
        {
            targetLocalAngles.z += swingOffset;
        }

        // 4. �������� ������������� ����������� ��������� ���� �������.
        // ��� ��� �� ������ transform ��������, ��� �������� ������� ��������� �� ���.
        transform.localEulerAngles = targetLocalAngles;
    }
}