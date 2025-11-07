using UnityEngine;

public class CollectablePickup : Pickup
{
    [SerializeField] private Collectable collectable;

    protected override void PickUp(InteractorController interactor)
    {
        CollectableHolder.Instance.AddCollectable(collectable);
    }
}
