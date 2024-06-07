using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class AdsManager : MonoBehaviour
{
    [SerializeField]
    private YandexGame yg;
    [SerializeField]
    private PlayerManager playerManager;
    public bool RewBtnClik;


    public void AdBtn()
    {
        YandexGame.RewVideoShow(100);
        RewBtnClik = true;
    }

    public void AdBtnCol()
    {
        playerManager.Diamonds = PlayerManager.numberOfCoins *= 2;

        // Сохраняем новое количество монет
        playerManager.Diamonds += PlayerPrefs.GetInt("Diamonds");
        PlayerPrefs.SetInt("Diamonds", playerManager.Diamonds);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}