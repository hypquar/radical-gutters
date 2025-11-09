using UnityEngine;

[CreateAssetMenu(fileName = "SceneActions", menuName = "Scriptable Objects/SceneActions")]
public class SceneActions : ScriptableObject
{
    public void QuitApplication()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay_Ready");
    }
}
