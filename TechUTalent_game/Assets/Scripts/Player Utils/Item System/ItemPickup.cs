using UnityEngine;

public class ItemPickup : Pickup
{
    [SerializeField] private Item item;

    protected override void PickUp(InteractorController interactor)
    {
        if (interactor.AdjacentInventory == null) return;
        var inventory = interactor.AdjacentInventory;
        ObjectiveManager.Instance.Collect();
        inventory.AddItem(item);
    }
}
