using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _InteractionUI;

    [Header("Text Elements")]
    [SerializeField] private TMPro.TextMeshProUGUI promptText;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowInteractionUI(bool show)
    {
        _InteractionUI?.SetActive(show);
    }
    public void SetPrompt(string text)
    {
        if (promptText != null)
        {
            promptText.text = text;
            _InteractionUI?.SetActive(!string.IsNullOrEmpty(text));
        }
    }
}
