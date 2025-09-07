using UnityEngine;
using UnityEngine.SceneManagement; // Эта строка ОБЯЗАТЕЛЬНА для работы со сценами

public class NextSceneLoader : MonoBehaviour
{
    // Флаг, чтобы избежать случайной многократной загрузки
    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в триггер вошел объект с тегом "Player"
        // и что мы еще не начали процесс загрузки
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;

            // Получаем индекс текущей сцены
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Вычисляем индекс следующей сцены
            int nextSceneIndex = currentSceneIndex + 1;

            // Проверяем, существует ли следующая сцена в настройках сборки.
            // SceneManager.sceneCountInBuildSettings содержит общее количество сцен.
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                // Если следующая сцена существует, загружаем ее
                Debug.Log($"Игрок вошел в зону. Загружается следующая сцена по индексу: {nextSceneIndex}");
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                // Если это была последняя сцена, можно загрузить первую (индекс 0)
                // или вывести сообщение об ошибке.
                Debug.Log("Это была последняя сцена. Загружается начальная сцена (индекс 0).");
                SceneManager.LoadScene(0);
            }
        }
    }
}