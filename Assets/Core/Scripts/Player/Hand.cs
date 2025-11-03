using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Transform handTransform; // Ссылка на трансформ руки
    private PickableItem currentItem;

    void Update()
    {
        // Обработка клавиш для броска/положить (G - положить, Q - бросить)
        if (currentItem != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropItem();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                ThrowItem();
            }
        }
    }

    public void PickUp(PickableItem item)
    {
        if (currentItem != null) return; // Уже что-то в руке

        currentItem = item;
        item.OnPickedUp(handTransform);

        // Можно добавить визуал, что предмет в руке
        Debug.Log("Подобрал предмет: " + item.name);
    }

    void DropItem()
    {
        if (currentItem == null) return;

        currentItem.OnDropped();
        Debug.Log("Положил предмет: " + currentItem.name);
        currentItem = null;
    }

    void ThrowItem()
    {
        if (currentItem == null) return;

        currentItem.OnThrown();
        Debug.Log("Бросил предмет: " + currentItem.name);
        currentItem = null;
    }

    // Для проверки, занята ли рука
    public bool IsHoldingItem()
    {
        return currentItem != null;
    }
}