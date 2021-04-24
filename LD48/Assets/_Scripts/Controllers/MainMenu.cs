using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   

    public void OnStartClick() {
        Debug.Log("Start clicked", gameObject);
        SceneManager.LoadScene(1);
    }

}
