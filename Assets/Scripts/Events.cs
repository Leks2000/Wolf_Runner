using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Events : MonoBehaviour
{
    public Text gameOverText;
    public Text speedText;
    public GameObject gameOverPanel;

    public void UnhideGameOverPanel()
    {
        this.gameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        this.gameOverPanel.SetActive(false);
    }
}
