using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private const string ScoreTable = "RunnerLB";

    [SerializeField]
    private GameObject MenuPanel;
    [SerializeField]
    private GameObject RewardBtn;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfCoins;
    public int Diamonds;
    public int Score;
    public float timer;

    public Text coinsText;
    public Text ScoreText;
    public Text speedText;

    public int speed;

    bool alreadyDone = false;
    bool TouchedToStart = false;

    [SerializeField]
    AudioManager audioManager;

    void Start()
    {
        timer = 0.0f;
        Score = 0;

        speed = 0;

        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    void Update()
    {
        UpdateScore();
        UpdateSpeed();
        if (numberOfCoins > 0)
        {
            Diamonds = numberOfCoins;
        }
        if (gameOver)
        {
            Time.timeScale = 0;
            if (!alreadyDone)
            {
                Events eventsObject = FindObjectOfType<Events>();
                eventsObject.UnhideGameOverPanel();
                audioManager.PauseAudio();
                if (numberOfCoins > 0)
                    RewardBtn.SetActive(true);
                alreadyDone = true;

                // Обновляем и сохраняем рекорд
                UpdateHighScore(Score);
            }
        }
        if (YandexGame.EnvironmentData.language == "ru")
        {
            coinsText.text = "Алмазы: " + numberOfCoins;
            speedText.text = "Скорость: " + FormatSpeedText();
            ScoreText.text = "Очки: " + Score;
        }
        else
        {
            coinsText.text = "Diamonds: " + numberOfCoins;
            speedText.text = "Speed: " + FormatSpeedText();
            ScoreText.text = "Score: " + Score;
        }
        StartCoroutine(StartGame());
    }

    void UpdateScore()
    {
        if (isGameStarted && !gameOver)
        {
            timer += Time.deltaTime;
            int seconds = Mathf.FloorToInt(timer);
            Score = (int)timer + (numberOfCoins * 2);
        }
    }

    void UpdateSpeed()
    {
        GameObject player = GameObject.Find("Player");
        PlayerController controller = player.GetComponent<PlayerController>();
        speed = (int)(controller.displayedSpeed);
    }

    string FormatSpeedText()
    {
        Color orange = new Color(1.0f, 0.64f, 0.0f);

        switch (speed)
        {
            case int s when (s < 75 && s >= 50):
                speedText.color = orange;
                speedText.fontSize = 55;
                break;

            case int s when (s <= 100 && s >= 75):
                speedText.color = Color.red;
                speedText.fontSize = 65;
                break;

            default:
                break;
        }
        if (YandexGame.EnvironmentData.language == "ru")
        {
            return string.Format("{0} км/ч", speed);
        }
        else
        {
            return string.Format("{0} km/h", speed);
        }
    }

    public void PlayAgain()
    {
        SaveDiamonds();
        YandexGame.FullscreenShow();
        LoadingManager.LoadingScene("Runner");
    }

    public void OpenMenu()
    {
        SaveDiamonds();
        LoadingManager.LoadingScene("MainMenu");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        MenuPanel.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        MenuPanel.SetActive(false);
    }

    private IEnumerator StartGame()
    {
        if (SwipeManager.tap)
        {
            if (!isGameStarted & !TouchedToStart)
            {
                var am = FindObjectOfType<AudioManager>();

                am.PlaySound("StartingUp");

                TouchedToStart = true;

                yield return new WaitForSeconds(1);

                isGameStarted = true;

                Destroy(startingText);
            }
        }
    }

    private void SaveDiamonds()
    {
        Diamonds += PlayerPrefs.GetInt("Diamonds");
        PlayerPrefs.SetInt("Diamonds", Diamonds);
        PlayerPrefs.Save();
    }

    private void UpdateHighScore(int currentScore)
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();

            // Отправляем новый рекорд в таблицу лидеров
            YandexGame.NewLeaderboardScores(ScoreTable, currentScore);
        }
    }

    private void OnApplicationQuit()
    {
        SaveDiamonds();
    }
}
