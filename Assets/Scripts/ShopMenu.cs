using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public Text priceText;
    private GameObject[] characters;
    private int[] characterPrices;
    private bool[] charactersPurchased;
    private int index;
    [SerializeField]
    private Text Diamonds;

    private int[] defaultCharacterPrices = { 0, 250, 500, 1000 };

    private void Start()
    {
        Time.timeScale = 1f;
        index = PlayerPrefs.GetInt("CharacterSelected", 0);
        characters = new GameObject[transform.childCount];
        characterPrices = defaultCharacterPrices;

        charactersPurchased = new bool[characterPrices.Length];

        // Загрузка информации о покупках из PlayerPrefs
        for (int i = 0; i < characterPrices.Length; i++)
        {
            if (i == 0) // Установите первый скин как купленный по умолчанию
            {
                charactersPurchased[i] = true;
                PlayerPrefs.SetInt("Character" + i + "Purchased", 1);
            }
            else
            {
                charactersPurchased[i] = PlayerPrefs.GetInt("Character" + i + "Purchased", 0) == 1;
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            characters[i] = transform.GetChild(i).gameObject;
        }

        foreach (GameObject go in characters)
        {
            go.SetActive(false);
        }

        if (characters[index])
        {
            characters[index].SetActive(true);
            UpdatePriceText();
        }
    }

    public void SelectLeft()
    {
        characters[index].SetActive(false);
        index--;
        if (index < 0)
        {
            index = characters.Length - 1;
        }
        characters[index].SetActive(true);
        UpdatePriceText();
    }

    public void SelectRight()
    {
        characters[index].SetActive(false);
        index++;
        if (index == characters.Length)
        {
            index = 0;
        }
        characters[index].SetActive(true);
        UpdatePriceText();
    }

    public void StartScene()
    {
        if (!charactersPurchased[index] && PlayerPrefs.GetInt("Diamonds") >= characterPrices[index])
        {
            charactersPurchased[index] = true;
            PlayerPrefs.SetInt("Character" + index + "Purchased", 1);
            PlayerPrefs.SetInt("CharacterSelected", index);
            PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") - characterPrices[index]);

            UpdatePriceText();
            SceneManager.LoadScene("LoadScene");
        }
        else if (charactersPurchased[index])
        {
            PlayerPrefs.SetInt("CharacterSelected", index);
            SceneManager.LoadScene("LoadScene");
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("LoadScene");
    }

    public void GetDiamonds()
    {
        Diamonds.text = PlayerPrefs.GetInt("Diamonds").ToString();
    }

    private void UpdatePriceText()
    {
        if (priceText != null)
        {
            if (charactersPurchased[index])
            {
                priceText.text = "";
            }
            else
            {
                priceText.text = characterPrices[index].ToString();
            }
        }
    }
}
