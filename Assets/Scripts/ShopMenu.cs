using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public Text priceText;
    private GameObject[] characters;
    private int[] characterPrices;
    private bool[] charactersPurchased; // Массив для хранения информации о покупке каждого персонажа
    private int index;

    // Массив цен для каждого персонажа
    private int[] defaultCharacterPrices = { 100, 150, 200 }; // Пример цен для 3 персонажей

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected", 0); // Если нет сохраненного значения, то по умолчанию выбирается первый персонаж
        characters = new GameObject[transform.childCount];
        characterPrices = defaultCharacterPrices; // Используем значения из массива defaultCharacterPrices

        charactersPurchased = new bool[characterPrices.Length]; // Создаем массив для хранения информации о покупке каждого персонажа

        // Загрузка информации о покупках из PlayerPrefs
        for (int i = 0; i < characterPrices.Length; i++)
        {
            charactersPurchased[i] = PlayerPrefs.GetInt("Character" + i + "Purchased", 0) == 1;
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
            if (priceText != null)
            {
                // Проверяем, куплен ли уже персонаж, и если да, то показываем пустую строку
                if (charactersPurchased[index])
                {
                    priceText.text = "";
                }
                else
                {
                    priceText.text = "" + characterPrices[index].ToString(); // Обновление текста с ценой
                }
            }
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
        if (priceText != null)
        {
            // Проверяем, куплен ли уже персонаж, и если да, то показываем пустую строку
            if (charactersPurchased[index])
            {
                priceText.text = "";
            }
            else
            {
                priceText.text = "" + characterPrices[index].ToString();
            }
        }
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
        if (priceText != null)
        {
            // Проверяем, куплен ли уже персонаж, и если да, то показываем пустую строку
            if (charactersPurchased[index])
            {
                priceText.text = "";
            }
            else
            {
                priceText.text = "" + characterPrices[index].ToString();
            }
        }
    }

    public void StartScene()
    {
        if (!charactersPurchased[index] && PlayerPrefs.GetInt("Diamonds") >= characterPrices[index])
        {
            charactersPurchased[index] = true; // Отмечаем персонажа как купленного
            PlayerPrefs.SetInt("Character" + index + "Purchased", 1); // Сохраняем информацию о покупке
            PlayerPrefs.SetInt("CharacterSelected", index);
            PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") - characterPrices[index]);

            // Обновляем текстовое поле с новой ценой или пустой строкой
            if (priceText != null)
            {
                priceText.text = "";
            }

            SceneManager.LoadScene(1);
        }
        else if (charactersPurchased[index])
        {
            // Если персонаж уже куплен, то загружаем сцену без вычитания алмазов
            PlayerPrefs.SetInt("CharacterSelected", index);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Not enough diamonds!");
        }
    }
}
