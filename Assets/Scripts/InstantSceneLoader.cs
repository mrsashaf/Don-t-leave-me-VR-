using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantSceneLoader : MonoBehaviour
{
    [Header("Настройки загрузки")]
    [Tooltip("Имя сцены, которую нужно мгновенно загрузить.")]
    public string sceneNameToLoad;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это игрок и что триггер еще не сработал
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("Мгновенная загрузка сцены: " + sceneNameToLoad);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}