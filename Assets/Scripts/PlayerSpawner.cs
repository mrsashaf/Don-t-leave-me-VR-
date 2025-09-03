using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPositionManager.HasSavedPosition)
        {
            Debug.Log("���������� ����������� ������. ���� ������ ��� ������.");

            // --- ������ �������� ---

            // ������� ������ �������� � ����� ����� �� ������ ����
            GameObject exitPortal = GameObject.FindGameObjectWithTag("PortalTrigger");

            if (exitPortal != null)
            {
                // ��������� �������� � ���������� �������

                // ������������ ������������� ������� ������� � �������, �� ��� �� ������ �������
                transform.position = exitPortal.transform.TransformPoint(PlayerPositionManager.RelativePosition);

                // ��������� ������������� �������� � �������� ������ �������
                transform.rotation = exitPortal.transform.rotation * PlayerPositionManager.RelativeRotation;

                Debug.Log("����� ��������� � ������������� ������� ����� �����.");
            }
            else
            {
                Debug.LogError("�� ������� ����� ������ � ����� 'PortalTrigger' � ����� �����! ����� �� ����� ��������������.");
            }

            // ���������� ���� � ����� ������, ����� �������� �������
            PlayerPositionManager.HasSavedPosition = false;
        }
    }
}