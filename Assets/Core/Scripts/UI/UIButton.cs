using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private bool _isPaused = false;

    [SerializeField] private PlayerManager _playerManager;
    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void RestartLVL()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void TogglePause()
    {
        _isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(_isPaused);

        Time.timeScale = _isPaused ? 0f : 1f;

        Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused;
        _playerManager.BlockAll();
    }
    public void ContinueGame()
    {
        _isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerManager.UnblockAll();
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }

}
