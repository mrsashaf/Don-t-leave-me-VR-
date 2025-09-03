using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantSceneLoader : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("��� �����, ������� ����� ��������� ���������.")]
    public string sceneNameToLoad;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ��� ����� � ��� ������� ��� �� ��������
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("���������� �������� �����: " + sceneNameToLoad);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}