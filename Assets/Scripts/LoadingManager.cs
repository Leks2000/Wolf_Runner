using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using YG;

public class LoadingManager : MonoBehaviour
{   
    [Header("Загружаемая сцена")]
    [Header("Остальные обьекты")]
    [SerializeField]
    private Image loadingImage;
    [SerializeField]
    private Text loadingText;

    private void Start()
    {
        Application.targetFrameRate = -1;
        Application.targetFrameRate = 60;
        if (loadingText != null)
        {
            StartCoroutine(UpdateLoadingText());
            StartCoroutine(LoadSceneAsync("Runner"));
        }
    }

    public static void LoadingScene(string nameScene)
    {
        SceneManager.LoadScene("LoadScene");
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
}
