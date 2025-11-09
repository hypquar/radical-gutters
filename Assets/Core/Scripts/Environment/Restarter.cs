using UnityEngine;

public class Restarter : MonoBehaviour
{
    public void RestartGame()
    {
        // Reload the current active scene to restart the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void ToEnd()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
    }
}
