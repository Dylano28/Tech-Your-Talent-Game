using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;

    private void Start()
    {
        GetComponent<Interactable>().onInteract.AddListener(PickUp);
    }


    private void PickUp(InteractorController interactor)
    {
        if (interactor.AdjacentInventory == null) return;
        var inventory = interactor.AdjacentInventory;
        inventory.AddItem(item);
    }
}
