using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuOptions : MonoBehaviour
{
    public GameObject loadButton;
    public int loadInt;

    void Start()
    {
        loadInt = PlayerPrefs.GetInt("AutoSave");
        if (loadInt > 0)
        {
            loadButton.SetActive(true);
        }
    }
    public void NewGameButton()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGameButton()
    {
        SceneManager.LoadScene(loadInt);
    }
    public void ExitButton()
    {
        Application.Quit();
    }

}
