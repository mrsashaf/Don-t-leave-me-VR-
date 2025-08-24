using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderWithDoors : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("��� �����, ������� ����� ���������.")]
    public string sceneNameToLoad;

    [Header("���������� �������")]
    [Tooltip("���������� ���� ��� �����, ������� ������ ��������� ��� ����� ������.")]
    public DoorRotationController[] doorsToClose;

    [Tooltip("�������� � �������� ����� �������� ������ ����� ��������� �����.")]
    public float delayBeforeLoading = 1.5f;

    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            StartCoroutine(CloseAndLoadScene());
        }
    }

    private IEnumerator CloseAndLoadScene()
    {
        Debug.Log("����� ����� � �������. ��������� �����...");

        // ���� ������� ���� ��������� ������ ���������
        foreach (DoorRotationController door in doorsToClose)
        {
            if (door != null)
            {
                // ������� ������� �����, ��� ����� ����� �� �� ����, ����� ��� ���������
                door.isPlayerInRange = false;
            }
        }

        // ���� �������� �����
        yield return new WaitForSeconds(delayBeforeLoading);

        Debug.Log("�������� ������. �������� �������� �����: " + sceneNameToLoad);

        // ��������� ����� �����
        SceneManager.LoadScene(sceneNameToLoad);
    }
}