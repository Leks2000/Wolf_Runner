using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [SerializeField]
    private LoadingManager loading;
    [SerializeField]
    private GameObject tutorialPanel;

    void Start()
    {
        ShowTutorial();
    }

    private void ShowTutorial()
    {
        if (!GameSession.TutorialShown)
        {
            tutorialPanel.SetActive(true);
            GameSession.TutorialShown = true;
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }
    public void Play()
    {
        LoadingManager.LoadingScene("Runner");
    }
}
public static class GameSession
{
    public static bool TutorialShown { get; set; } = false;
}

