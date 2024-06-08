using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("Загружаемая сцена")]
    [Header("Остальные обьекты")]
    [SerializeField]
    private Image loadingImage;
    [SerializeField]
    private Text loadingText;

    private static string sceneToLoad; // Статическая переменная для хранения имени сцены

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (loadingText != null)
        {
            StartCoroutine(UpdateLoadingText());
            StartCoroutine(LoadSceneAsync(sceneToLoad)); // Используем сохранённое имя сцены
        }
    }

    public static void LoadingScene(string nameScene)
    {
        sceneToLoad = nameScene; // Сохраняем имя сцены для загрузки
        SceneManager.LoadScene("LoadScene"); // Загружаем сцену загрузки
    }

    IEnumerator LoadSceneAsync(string nameScene)
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(nameScene);

        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            if (loadingImage != null)
            {
                loadingImage.fillAmount = progress;
            }
            yield return null;
        }
    }

    IEnumerator UpdateLoadingText()
    {
        string baseText = "Loading";
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount + 1) % 4; // количество точек от 0 до 3
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f); // время обновления текста
        }
    }
    public IEnumerator LoadMenuSceneAfterSound(string Scene)
    {
        yield return new WaitForSeconds(0.5f);
        LoadingScene(Scene);
    }
}
