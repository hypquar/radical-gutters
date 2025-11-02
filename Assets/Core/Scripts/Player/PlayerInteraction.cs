using UnityEngine; 
using TMPro;
using System.Xml.Serialization;
public class PlayerInteraction : MonoBehaviour 
{ 

    [SerializeField] private Camera _PlayerCamera; 

    [SerializeField] private float _InteractionDistance = 10f;

    [SerializeField] private UIManager _PlayerUIManager;

    [SerializeField] private bool _IsRayActive = true;
    private void Update() 
    {
        if (_IsRayActive)
        {
            Ray ray = _PlayerCamera.ViewportPointToRay(Vector3.one / 2f);
            if (Physics.Raycast(ray, out RaycastHit hit, _InteractionDistance))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    string prompt = interactable.GetPrompt();

                    bool hasPrompt = !string.IsNullOrEmpty(prompt);

                    _PlayerUIManager.ShowInteractionUI(hasPrompt);

                    _PlayerUIManager.SetPrompt(prompt);

                    if (Input.GetKeyDown(KeyCode.E)) interactable.Interact(); return;
                }
            }

            _PlayerUIManager.ShowInteractionUI(false);

        }

        else _PlayerUIManager.ShowInteractionUI(false);
    }
    
    public void BlockRay() => _IsRayActive = false;
    
    public void UnblockRay() => _IsRayActive = true;
    
}