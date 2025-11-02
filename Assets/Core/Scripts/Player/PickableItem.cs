using UnityEngine;
using UnityEngine.XR;


public class PickableItem : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public string GetPrompt()
    {
        return "Подобрать";
    }

    public void Interact()
    {
        // Находим руку в сцене
        Hand hand = FindObjectOfType<Hand>();
        if (hand != null && !hand.IsHoldingItem())
        {
            hand.PickUp(this);
        }
    }

    // Вызывается Hand'ом
    public void OnPickedUp(Transform handTransform)
    {

        // Выключаем физику
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        if (col != null) col.enabled = false;

        // Прикрепляем к руке
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDropped()
    {
        ResetPhysics();
        transform.SetParent(null);
    }

    public void OnThrown()
    {
        ResetPhysics();
        transform.SetParent(null);

        // Добавляем силу броска вперед от камеры
        if (rb != null)
        {
            Vector3 throwDirection = Camera.main.transform.forward;
            rb.AddForce(throwDirection * 15f, ForceMode.Impulse);
        }
    }

    private void ResetPhysics()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
        if (col != null) col.enabled = true;
    }
}

