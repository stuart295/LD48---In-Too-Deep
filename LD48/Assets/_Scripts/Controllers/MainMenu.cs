using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject tutorial;
   

    public void OnStartClick() {
        Debug.Log("Start clicked", gameObject);
        ShowTutorial();
    }

    public void OnExitClick() {
        Debug.Log("Exit clicked", gameObject);
        Application.Quit();
    }

    public void ShowTutorial() {
        mainMenu.SetActive(false);
        tutorial.SetActive(true);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

}
