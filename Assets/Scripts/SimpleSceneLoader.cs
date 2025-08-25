using UnityEngine;
using UnityEngine.SceneManagement; // Эта строка ОБЯЗАТЕЛЬНА для работы со сценами

public class SimpleSceneLoader : MonoBehaviour
{
    [Header("Настройки загрузки")]
    [Tooltip("Точное имя сцены, которую нужно загрузить. Имя должно совпадать с названием файла сцены.")]
    public string sceneNameToLoad;

    // Флаг, чтобы избежать случайной многократной загрузки
    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в триггер вошел объект с тегом "Player"
        // и что мы еще не начали процесс загрузки
        if (other.CompareTag("Player") && !isLoading)
        {
            // Устанавливаем флаг, чтобы этот код не сработал снова
            isLoading = true;

            // Выводим сообщение в консоль, чтобы было понятно, что скрипт сработал
            Debug.Log($"Игрок вошел в зону. Загружается сцена: {sceneNameToLoad}");

            // Главная команда - загрузить сцену по ее имени
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}