using UnityEngine;

public class RockTrigger : MonoBehaviour
{
    public bool IsRightRock;
    [SerializeField] private RockManager _rockManager;
    [SerializeField] private string _rockTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_rockTag))
        {
            IsRightRock = true;
            _rockManager.CheckRocksStatus();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsRightRock = false;
    }
}
