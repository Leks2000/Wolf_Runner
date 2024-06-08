using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [SerializeField]
    private LoadingManager loading;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void Play()
    {
        LoadingManager.LoadingScene("Runner");
    }
}
