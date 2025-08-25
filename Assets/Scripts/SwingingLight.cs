using UnityEngine;

public class SwingingLight : MonoBehaviour
{
    public enum SwingAxis { X, Z } // ���, �� ������� ����� ����������� �������

    [Header("��������� �������")]
    [Tooltip("������������ ���� ������� � �������� � ������ �������.")]
    [Range(0f, 90f)]
    public float maxSwingAngle = 15f;

    [Tooltip("�������� �������. ��� ���� ��������, ��� �������.")]
    public float swingSpeed = 1f;

    [Tooltip("���, �� ������� ����� �������� ����� (X = ������-�����, Z = ������-�����).")]
    public SwingAxis swingAxis = SwingAxis.Z;

    [Header("�����������")]
    [Tooltip("��������� ��������� ����������� � �������� � ����, ����� ����� �� �������� ���������.")]
    public bool addRandomness = true;

    // ��������� ���������� ��� �������� ��������� ��������
    private Quaternion initialRotation;
    private float randomOffset;

    void Start()
    {
        // ���������� ����������� ������� �������
        initialRotation = transform.rotation;

        // ���� �������� �����������, ��������� ���������� �������� ��� ���� �����
        if (addRandomness)
        {
            // ��������� �����, ������� ������� �������� ����������
            randomOffset = Random.Range(0f, 100f);
        }
    }

    void Update()
    {
        // 1. ��������� ������� ���� � ������� ���������
        // Mathf.Sin(time) ������� ������� ��������� ����� -1 � 1
        float time = Time.time * swingSpeed + randomOffset;
        float currentAngle = maxSwingAngle * Mathf.Sin(time);

        // 2. ������� �������� �� ������ ������������ ����
        Quaternion swingRotation;

        // �������� ��� �������� � ����������� �� ��������� � ����������
        if (swingAxis == SwingAxis.X)
        {
            // �������� ������-�����
            swingRotation = Quaternion.Euler(currentAngle, 0f, 0f);
        }
        else // swingAxis == SwingAxis.Z
        {
            // �������� ������-�����
            swingRotation = Quaternion.Euler(0f, 0f, currentAngle);
        }

        // 3. ��������� ����� �������� � ������������
        // �� ��������, ����� ��������� ��������� ������� � ��������� �������� ��������
        transform.rotation = initialRotation * swingRotation;
    }
}