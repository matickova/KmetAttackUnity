using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("new_controller"); // ime scene, ki jo želiš naložiti
    }

    public void QuitGame()
    {
        Application.Quit(); // v editorju ne dela, samo v build-u
        Debug.Log("Quit pressed");
    }
}

