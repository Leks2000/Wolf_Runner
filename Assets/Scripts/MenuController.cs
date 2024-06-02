using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject Wolf;

    void Update()
    {
        Wolf.transform.Rotate(0, 0, 0.1f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Runner");
    }
}
