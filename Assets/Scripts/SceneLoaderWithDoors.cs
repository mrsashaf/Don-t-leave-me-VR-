using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderWithDoors : MonoBehaviour
{
    [Header("Настройки загрузки")]
    [Tooltip("Имя сцены, которую нужно загрузить.")]
    public string sceneNameToLoad;

    [Header("Управление дверями")]
    [Tooltip("Перетащите сюда все двери, которые должны закрыться при входе игрока.")]
    public DoorRotationController[] doorsToClose;

    [Tooltip("Задержка в секундах после закрытия дверей перед загрузкой сцены.")]
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
        Debug.Log("Игрок вошел в триггер. Закрываем двери...");

        // Даем команду всем указанным дверям закрыться
        foreach (DoorRotationController door in doorsToClose)
        {
            if (door != null)
            {
                // Говорим скрипту двери, что игрок вышел из ее зоны, чтобы она закрылась
                door.isPlayerInRange = false;
            }
        }

        // Ждем заданное время
        yield return new WaitForSeconds(delayBeforeLoading);

        Debug.Log("Задержка прошла. Начинаем загрузку сцены: " + sceneNameToLoad);

        // Загружаем новую сцену
        SceneManager.LoadScene(sceneNameToLoad);
    }
}