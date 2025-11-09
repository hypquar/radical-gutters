using UnityEngine;
using UnityEngine.Events;

public class Exfil : MonoBehaviour
{
    public UnityEvent OnExfil;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Exfil triggered by Player");
            OnExfil?.Invoke();
        }
    }
}
