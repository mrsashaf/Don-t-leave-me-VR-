using UnityEngine;
using UnityEngine.SceneManagement; // ��� ������ ����������� ��� ������ �� �������

public class SimpleSceneLoader : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("������ ��� �����, ������� ����� ���������. ��� ������ ��������� � ��������� ����� �����.")]
    public string sceneNameToLoad;

    // ����, ����� �������� ��������� ������������ ��������
    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� � ������� ����� ������ � ����� "Player"
        // � ��� �� ��� �� ������ ������� ��������
        if (other.CompareTag("Player") && !isLoading)
        {
            // ������������� ����, ����� ���� ��� �� �������� �����
            isLoading = true;

            // ������� ��������� � �������, ����� ���� �������, ��� ������ ��������
            Debug.Log($"����� ����� � ����. ����������� �����: {sceneNameToLoad}");

            // ������� ������� - ��������� ����� �� �� �����
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}