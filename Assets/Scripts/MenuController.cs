using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Text Diamonds;

    public void StartGame()
    {
        SceneManager.LoadScene("Runner");
    }

    public void GetDiamonds()
    {
        Diamonds.text = PlayerPrefs.GetInt("Diamonds").ToString();
    }
}
