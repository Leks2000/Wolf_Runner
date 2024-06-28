using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class AdsManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager playerManager;

    public void AdBtn()
    {
        YandexGame.RewVideoShow(100);
        Cursor.visible = false;
    }

    public void AdBtnCol()
    {
        LoadingManager.LoadingScene("MainMenu");
        Cursor.visible = true;
        playerManager.Diamonds = PlayerManager.numberOfCoins *= 2;
        playerManager.Diamonds += PlayerPrefs.GetInt("Diamonds");
        PlayerPrefs.SetInt("Diamonds", playerManager.Diamonds);
        PlayerPrefs.Save();
    }
}